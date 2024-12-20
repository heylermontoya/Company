using Company.Domain.Exceptions;
using Company.Infrastructure;
using System.Transactions;
using Company.Domain.Ports;
using Transaction = Company.Infrastructure.Transaction;

namespace Company.Domain.Services
{
    [DomainService]
    public class TransactionService
    {
        protected readonly IGenericRepository<Transaction> TransactionRepository;
        protected readonly IGenericRepository<Role> RoleRepository;
        private readonly UserService userService;
        private readonly ProductService productService;

        public TransactionService(
            IGenericRepository<Transaction> TransactionRepository,
            IGenericRepository<Role> RoleRepository,
            UserService userService,
            ProductService productService
        )
        {
            this.TransactionRepository = TransactionRepository;
            this.RoleRepository = RoleRepository;
            this.userService = userService;
            this.productService = productService;

        }

        public async Task<List<Transaction>> GetTransactionsAllAsync()
        {
            IEnumerable<Transaction> transactions =  await TransactionRepository.GetAsync(
                transaction => !transaction.Isdeleted,
                includeStringProperties:
                        $"{nameof(Transaction.Product)}," +
                        $"{nameof(Transaction.User)}," +
                        $"{nameof(Transaction.User)}.{nameof(User.Usersinroles)}," +
                        $"{nameof(Transaction.User)}.{nameof(User.Usersinroles)}.{nameof(Usersinrole.Role)}"
            );

            return transactions.ToList();
        }
        
        public async Task<List<Transaction>> GetTransactionsByProductAsync(int productId)
        {
            //Valida que el producto exista
            await productService.GetProductByProductIdAsync(productId);

            IEnumerable<Transaction> transactions =  await TransactionRepository.GetAsync(
                transaction => transaction.Productid == productId && !transaction.Isdeleted,
                includeStringProperties:
                        $"{nameof(Transaction.Product)}," +
                        $"{nameof(Transaction.User)}," +
                        $"{nameof(Transaction.User)}.{nameof(User.Usersinroles)}," +
                        $"{nameof(Transaction.User)}.{nameof(User.Usersinroles)}.{nameof(Usersinrole.Role)}"
            );

            return transactions.ToList();
        }
        
        public async Task<List<Transaction>> GetTransactionsByUserIdAsync(int userId)
        {
            //Valida que el usuario exista
            await userService.GetUserByUserIdAsync(userId);

            IEnumerable<Transaction> transactions =  await TransactionRepository.GetAsync(
                transaction => transaction.Userid == userId && !transaction.Isdeleted,
                includeStringProperties:
                        $"{nameof(Transaction.Product)}," +
                        $"{nameof(Transaction.User)}," +
                        $"{nameof(Transaction.User)}.{nameof(User.Usersinroles)}," +
                        $"{nameof(Transaction.User)}.{nameof(User.Usersinroles)}.{nameof(Usersinrole.Role)}"
            );

            return transactions.ToList();
        }

        public async Task<Transaction> CreateTransactionAsync(
            int productId,
            int userId,
            int quantity
        )
        {
            // Validar usuario y roles permitidos
            User user = await userService.GetUserByUserIdAsync( userId );
            await ValidateRolesCreateTransactionAsync(user.Usersinroles.ToList());

            // Validar producto e inventario
            Product product = await productService.GetProductByProductIdAsync( productId );
            ValidateProductInventory(product, quantity);            

            // Crear transaction
            Transaction transaction = new()
            {
                Productid = productId,
                Userid = userId,
                Quantity = quantity                
            };

            // Transacción
            using TransactionScope scope = new(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                transaction = await TransactionRepository.AddAsync(transaction);

                // Descontar inventario del producto
                int newInventory = product.Inventory - quantity;
                await productService.UpdateProductAsync(
                    productId,
                    product.Productname,
                    newInventory,
                    product.Price
                );

                // Recuperar usuario con roles
                transaction = await TransactionRepository.FindByAlternateKeyAsync(
                    t => t.Transactionid == transaction.Transactionid,
                    includeProperties:
                        $"{nameof(Transaction.Product)}," +
                        $"{nameof(Transaction.User)}," +
                        $"{nameof(Transaction.User)}.{nameof(User.Usersinroles)}," +
                        $"{nameof(Transaction.User)}.{nameof(User.Usersinroles)}.{nameof(Usersinrole.Role)}"
                );

                scope.Complete();
                return transaction;
            }
            catch (Exception ex)
            {
                throw new AppException($"Error al crear la transaction: {ex.Message}", ex);
            }
        }

        public async Task DeleteTransactionAsync(int transactionId, int userId)
        {
            // Verificar si la transaccion existe
            Transaction transaction = await TransactionRepository.GetByIdAsync(transactionId)
                ?? throw new AppException($"La transaccion con ID {transactionId} no existe.");

            if (transaction.Isdeleted)
            {
                throw new AppException($"La transaccion con ID {transactionId} fue eliminado anteriormente.");
            }

            // Validar usuario y roles permitidos
            User user = await userService.GetUserByUserIdAsync(userId);
            await ValidateRolesDeleteTransactionAsync(user.Usersinroles.ToList());
            
            // Marcar como eliminado
            transaction.Isdeleted = true;

            Product product = await productService.GetProductByProductIdAsync(transaction.Productid);

            // Transacción
            using TransactionScope scope = new(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                // Aumentar inventario del producto
                int newInventory = product.Inventory + transaction.Quantity;
                await productService.UpdateProductAsync(
                    product.Productid,
                    product.Productname,
                    newInventory,
                    product.Price
                );

                // Actualizar el producto en el repositorio
                await TransactionRepository.UpdateAsync(transaction);

                scope.Complete();
            }
            catch (Exception ex)
            {
                throw new AppException($"Error al eliminar la transaction: {ex.Message}", ex);
            }
        }

        private async Task ValidateRolesCreateTransactionAsync(List<Usersinrole> usersInRoles)
        {
            // Obtener los roles permitidos para crear transacciones
            IEnumerable<Role> roles = await RoleRepository.GetAsync(r => r.Cancreatetransaction);

            // Obtener los IDs de los roles permitidos
            HashSet<int> allowedRoleIds = roles.Select(r => r.Roleid).ToHashSet();

            // Verificar si algún rol en usersInRoles está en los roles permitidos
            bool hasPermission = usersInRoles.Any(uir => allowedRoleIds.Contains(uir.Roleid));

            if (!hasPermission)
            {
                throw new AppException("El usuario no tiene permisos para crear transacciones.");
            }
        }

        private async Task ValidateRolesDeleteTransactionAsync(List<Usersinrole> usersInRoles)
        {
            // Obtener los roles permitidos para crear transacciones
            IEnumerable<Role> roles = await RoleRepository.GetAsync(r => r.Candeletetransaction);

            // Obtener los IDs de los roles permitidos
            HashSet<int> allowedRoleIds = roles.Select(r => r.Roleid).ToHashSet();

            // Verificar si algún rol en usersInRoles está en los roles permitidos
            bool hasPermission = usersInRoles.Any(uir => allowedRoleIds.Contains(uir.Roleid));

            if (!hasPermission)
            {
                throw new AppException("El usuario no tiene permisos para eliminar transacciones.");
            }
        }

        private static void ValidateProductInventory(Product product, int quantity)
        {
            if (quantity > product.Inventory)
            {
                throw new AppException("La cantidad solicitada supera el inventario disponible del producto.");
            }
        }
    }
}
