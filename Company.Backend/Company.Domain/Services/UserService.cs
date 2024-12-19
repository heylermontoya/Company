using Company.Domain.Exceptions;
using Company.Domain.Ports;
using Company.Infrastructure;
using System.Transactions;

namespace Company.Domain.Services
{
    [DomainService]
    public class UserService
    {
        
        protected readonly IGenericRepository<User> UserRepository;
        protected readonly IGenericRepository<Role> RoleRepository;
        protected readonly IGenericRepository<Usersinrole> UsersInRolepository;

        public UserService(
            IGenericRepository<Role> Rolerepository,
            IGenericRepository<Usersinrole> UsersInRolepository,
            IGenericRepository<User> UserRepository
        )
        {
            this.UserRepository = UserRepository;
            this.UsersInRolepository = UsersInRolepository;
            this.RoleRepository = Rolerepository;
        }

        public async Task<User> CreateUserAsync(
            string userName,
            string email,
            string passwordHash,
            List<string> roles
        )
        {
            // Validación de roles
            List<Role> listRole = await ValidateRolesAsync(roles);

            // Validación de usuario duplicado
            await ValidateDuplicateUserAsync(userName, email);

            // Crear usuario
            User user = new()
            {
                Username = userName,
                Email = email,
                Passwordhash = passwordHash
            };

            // Transacción
            using TransactionScope scope = new(TransactionScopeAsyncFlowOption.Enabled);
            try
            {                
                user = await UserRepository.AddAsync(user);

                // Asignar roles al usuario
                await AssignRolesToUserAsync(user, listRole);

                // Recuperar usuario con roles
                user = await UserRepository.FindByAlternateKeyAsync(
                    u => u.Userid == user.Userid,
                    includeProperties:
                        $"{nameof(user.Usersinroles)}," +
                        $"{nameof(user.Usersinroles)}.{nameof(Usersinrole.Role)}"
                );

                scope.Complete();
                return user;
            }
            catch (Exception ex)
            {
                throw new AppException($"Error al crear el usuario: {ex.Message}", ex);
            }
        }
        
        public async Task<User> UpdateUserAsync(
            int userId,
            string userName,
            string email,
            string passwordHash,
            List<string> roles
        )
        {
            // Validación de roles
            List<Role> listRole = await ValidateRolesAsync(roles);

            // Validación de usuario duplicado
            await ValidateDuplicateUserAsync(userName, email);

            // Update usuario
            User user = await UserRepository.GetByIdAsync(userId);
            user.Username = userName;
            user.Email = email;
            user.Passwordhash = passwordHash;
            

            // Transacción
            using TransactionScope scope = new(TransactionScopeAsyncFlowOption.Enabled);
            try
            {                
                user = await UserRepository.UpdateAsync(user);

                // Actualizar roles al usuario
                await UsersInRolepository.DeleteAsync(userInRole => userInRole.Userid == userId);
                await AssignRolesToUserAsync(user, listRole);

                // Recuperar usuario con roles
                user = await UserRepository.FindByAlternateKeyAsync(
                    u => u.Userid == user.Userid,
                    includeProperties:
                        $"{nameof(user.Usersinroles)}," +
                        $"{nameof(user.Usersinroles)}.{nameof(Usersinrole.Role)}"
                );

                scope.Complete();
                return user;
            }
            catch (Exception ex)
            {
                throw new AppException($"Error al crear el usuario: {ex.Message}", ex);
            }
        }

        private async Task<List<Role>> ValidateRolesAsync(IEnumerable<string> roles)
        {
            List<Role> listRole = [];

            foreach (string roleName in roles)
            {
                Role? role = (
                    await RoleRepository
                    .GetAsync(r => r.Rolename == roleName)
                ).FirstOrDefault();

                if (role == null)
                {
                    throw new AppException($"El rol '{roleName}' no es válido.");
                }

                listRole.Add(role);
            }

            return listRole;
        }

        private async Task ValidateDuplicateUserAsync(string userName, string email)
        {
            IEnumerable<User> listUsers = await UserRepository.GetAsync();

            if (listUsers.Any(user => user.Username == userName))
            {
                throw new AppException($"Ya existe un usuario con el nombre de usuario '{userName}'.");
            }
            if (listUsers.Any(user => user.Email == email))
            {
                throw new AppException($"Ya existe un usuario con el email '{email}'.");
            }
        }

        private async Task AssignRolesToUserAsync(User user, IEnumerable<Role> roles)
        {
            IEnumerable<Usersinrole> usersInRoles = roles.Select(role => new Usersinrole
            {
                Userid = user.Userid,
                Roleid = role.Roleid
            });

            foreach (Usersinrole usersInRole in usersInRoles)
            {
                await UsersInRolepository.AddAsync(usersInRole);
            }
        }
    }
}
