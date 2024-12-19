using Company.Domain.Exceptions;
using Company.Domain.Ports;
using Company.Infrastructure;

namespace Company.Domain.Services
{
    [DomainService]
    public class ProductService
    {
        protected readonly IGenericRepository<Product> ProductRepository;

        public ProductService(
            IGenericRepository<Product> ProductRepository
        )
        {
            this.ProductRepository = ProductRepository;
        }

        public async Task<Product> CreateProductAsync(
            string productName,
            int inventory,
            decimal price
        )
        {
            // Validación de Product duplicado
            await ValidateDuplicateProductAsync(productName);

            // Validar datos del producto
            ValidateProductData(inventory, price);

            //se crea objeto Product y se agrega
            Product product = new()
            {
                Productname = productName,
                Inventory = inventory,
                Price = price,
                Isdeleted = false
            };

            product = await ProductRepository.AddAsync(product);

            return product;
        }

        public async Task<Product> UpdateProductAsync(
            int productId,
            string productName,
            int inventory,
            decimal price
        )
        {

            // Verificar existencia del producto
            Product? product = await ProductRepository.GetByIdAsync(productId)
                ?? throw new AppException($"El producto con ID {productId} no existe.");

            if (product.Isdeleted)
            {
                throw new AppException($"El producto con ID {productId} fue eliminado anteriormente.");
            }

            // Validar producto duplicado (excluyendo el actual)
            await ValidateDuplicateProductAsync(productName, productId);

            // Validar datos del producto
            ValidateProductData(inventory, price);

            // Actualizar el producto
            product.Productname = productName;
            product.Inventory = inventory;
            product.Price = price;

            product = await ProductRepository.UpdateAsync(product);

            return product;
        }

        public async Task DeleteProductAsync(int productId)
        {
            // Verificar si el producto existe
            var product = await ProductRepository.GetByIdAsync(productId)
                ?? throw new AppException($"El producto con ID {productId} no existe.");

            if (product.Isdeleted)
            {
                throw new AppException($"El producto con ID {productId} fue eliminado anteriormente.");
            }

            // Marcar como eliminado
            product.Isdeleted = true;

            // Actualizar el producto en el repositorio
            await ProductRepository.UpdateAsync(product);
        }

        private async Task ValidateDuplicateProductAsync(string productName, int? excludeProductId = null)
        {
            IEnumerable<Product> listproducts = await ProductRepository.GetAsync();

            if (listproducts.Any(product => product.Productname == productName && product.Productid != excludeProductId && !product.Isdeleted))
            {
                throw new AppException($"Ya existe un producto con el nombre '{productName}'.");
            }
        }

        private static void ValidateProductData(int inventory, decimal price)
        {
            if (inventory < 0)
            {
                throw new AppException("El inventario debe ser mayor o igual a cero.");
            }

            if (price < 0)
            {
                throw new AppException("El precio debe ser mayor o igual a cero.");
            }
        }
    }
}
