using Company.Domain.Exceptions;
using Company.Domain.Ports;
using Company.Domain.Services;
using Company.Domain.Tests.DataBuilder;
using Company.Infrastructure;
using NSubstitute;
using System.Linq.Expressions;

namespace Company.Domain.Tests
{
    [TestClass]
    public class ProductServiceTest
    {
        private ProductService Service { get; set; } = default!;
        private IGenericRepository<Product> ProductRepository { get; set; } = default!;
        private ProductBuilder ProductBuilder { get; set; } = default!;

        [TestInitialize]
        public void Initialize()
        {
            ProductRepository = Substitute.For<IGenericRepository<Product>>();

            Service = new(
                ProductRepository
            );

            ProductBuilder = new();
        }

        [TestMethod]
        public async Task GetProductsAllAsync_Ok()
        {
            //Arrange
            List<Product> product = [
                ProductBuilder
                    .Build()
            ];
                
            ProductRepository.GetAsync(
                product => !product.Isdeleted
            ).ReturnsForAnyArgs(product);

            //Act
            List<Product> response = await Service.GetProductsAllAsync();

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(product.Count, response.Count);
            await ProductRepository
                .ReceivedWithAnyArgs(1)
                .GetAsync(
                    Arg.Any<Expression<Func<Product, bool>>?>()
                );
        }
        
        [TestMethod]
        public async Task GetProductByProductIdAsync_Ok()
        {
            //Arrange
            int productId = 1;
            Product product = ProductBuilder
                .WithProductId(productId)
                .Build();
                            
            ProductRepository.FindByAlternateKeyAsync(
                Arg.Any<Expression < Func<Product, bool>>>()
            ).ReturnsForAnyArgs(product);

            //Act
            Product response = await Service.GetProductByProductIdAsync(productId);

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(product.Productid, response.Productid);
            await ProductRepository
                .ReceivedWithAnyArgs(1)
                .FindByAlternateKeyAsync(
                    Arg.Any<Expression<Func<Product, bool>>>()
                );
        }
        
        [TestMethod]
        public async Task GetProductByProductIdAsync_Failed()
        {
            //Arrange
            int productId = 1;

            //Act
            AppException ex = await Assert.ThrowsExceptionAsync<AppException>(async () =>
            {
                await Service.GetProductByProductIdAsync(productId);
            });

            //Assert
            Assert.AreEqual(
                $"El producto con ID {productId} no existe.", 
                ex.Message
            );            
        }

        [TestMethod]
        public async Task CreateProductAsync_Ok()
        {
            //Arrange
            string productName = "Caja 1";
            int inventory = 10;
            decimal price = 1000;
            int productId = 1;

            Product product = ProductBuilder
                .WithProductId(productId)
                .Build();

            IEnumerable<Product> listproducts = [product];
                
            ProductRepository.GetAsync().ReturnsForAnyArgs(listproducts);
            ProductRepository.AddAsync(product).ReturnsForAnyArgs(product);

            //Act
            Product response = await Service.CreateProductAsync(productName,inventory,price);

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(product.Productid, response.Productid);
            Assert.AreEqual(product.Productname, response.Productname);
            Assert.AreEqual(product.Inventory, response.Inventory);
            Assert.AreEqual(product.Price, response.Price);
            Assert.IsFalse(response.Isdeleted);
            await ProductRepository.ReceivedWithAnyArgs(1).GetAsync();
            await ProductRepository.ReceivedWithAnyArgs(1).AddAsync(Arg.Any<Product>());
        }

        [TestMethod]
        public async Task CreateProductAsync_ProductExistedFailed()
        {
            //Arrange
            string productName = "Caja 1";
            int inventory = 10;
            decimal price = 1000;
            int productId = 1;

            Product product = ProductBuilder
                .WithProductName(productName)
                .WithProductId(productId)
                .Build();

            IEnumerable<Product> listproducts = [product];

            ProductRepository.GetAsync().ReturnsForAnyArgs(listproducts);

            //Act
            AppException ex = await Assert.ThrowsExceptionAsync<AppException>(async () =>
            {
                await Service.CreateProductAsync(productName, inventory, price);
            });

            //Assert
            Assert.AreEqual(
                $"Ya existe un producto con el nombre '{productName}'.",
                ex.Message
            );
            await ProductRepository.ReceivedWithAnyArgs(1).GetAsync();
        }
        
        [TestMethod]
        public async Task CreateProductAsync_ProductWithPriceFailed()
        {
            //Arrange
            string productName = "Caja 1";
            int inventory = 10;
            decimal price = -1000;
            int productId = 1;

            Product product = ProductBuilder
                .WithProductId(productId)
                .Build();

            IEnumerable<Product> listproducts = [product];

            ProductRepository.GetAsync().ReturnsForAnyArgs(listproducts);

            //Act
            AppException ex = await Assert.ThrowsExceptionAsync<AppException>(async () =>
            {
                await Service.CreateProductAsync(productName, inventory, price);
            });

            //Assert
            Assert.AreEqual(
                "El precio debe ser mayor o igual a cero.",
                ex.Message
            );
            await ProductRepository.ReceivedWithAnyArgs(1).GetAsync();
        }
        
        [TestMethod]
        public async Task CreateProductAsync_ProductWithInventoryFailed()
        {
            //Arrange
            string productName = "Caja 1";
            int inventory = -10;
            decimal price = 1000;
            int productId = 1;

            Product product = ProductBuilder
                .WithProductId(productId)
                .Build();

            IEnumerable<Product> listproducts = [product];

            ProductRepository.GetAsync().ReturnsForAnyArgs(listproducts);

            //Act
            AppException ex = await Assert.ThrowsExceptionAsync<AppException>(async () =>
            {
                await Service.CreateProductAsync(productName, inventory, price);
            });

            //Assert
            Assert.AreEqual(
                "El inventario debe ser mayor o igual a cero.",
                ex.Message
            );
            await ProductRepository.ReceivedWithAnyArgs(1).GetAsync();
        }

        [TestMethod]
        public async Task UpdateProductAsync_Ok()
        {
            //Arrange
            string productName = "Caja 1";
            int inventory = 10;
            decimal price = 1000;
            int productId = 1;

            Product product = ProductBuilder
                .WithProductId(productId)
                .Build();

            IEnumerable<Product> listproducts = [product];

            ProductRepository.GetByIdAsync(productId).ReturnsForAnyArgs(product);
            ProductRepository.GetAsync().ReturnsForAnyArgs(listproducts);
            ProductRepository.UpdateAsync(product).ReturnsForAnyArgs(product);

            //Act
            Product response = await Service.UpdateProductAsync(
                productId,
                productName, 
                inventory, 
                price
            );

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(product.Productid, response.Productid);
            Assert.AreEqual(product.Productname, response.Productname);
            Assert.AreEqual(product.Inventory, response.Inventory);
            Assert.AreEqual(product.Price, response.Price);
            Assert.IsFalse(response.Isdeleted);
            await ProductRepository.ReceivedWithAnyArgs(1).GetByIdAsync(Arg.Any<object>());
            await ProductRepository.ReceivedWithAnyArgs(1).GetAsync();
            await ProductRepository.ReceivedWithAnyArgs(1).UpdateAsync(Arg.Any<Product>());
        }
        
        [TestMethod]
        public async Task UpdateProductAsync_ProductNotExistFailed()
        {
            //Arrange
            string productName = "Caja 1";
            int inventory = 10;
            decimal price = 1000;
            int productId = 1;
           
            //Act            
            AppException ex = await Assert.ThrowsExceptionAsync<AppException>(async () =>
            {
                await Service.UpdateProductAsync(
                   productId,
                   productName,
                   inventory,
                   price
                );
            });

            //Assert
            Assert.AreEqual(
                $"El producto con ID {productId} no existe.",
                ex.Message
            );            
        }
        
        [TestMethod]
        public async Task UpdateProductAsync_ProductBeforeDeletedFailed()
        {
            //Arrange
            string productName = "Caja 1";
            int inventory = 10;
            decimal price = 1000;
            int productId = 1;

            Product product = ProductBuilder
                .WithProductId(productId)
                .WithIsDeleted(true)
                .Build();

            ProductRepository.GetByIdAsync(productId).ReturnsForAnyArgs(product);
            
            //Act            
            AppException ex = await Assert.ThrowsExceptionAsync<AppException>(async () =>
            {
                await Service.UpdateProductAsync(
                   productId,
                   productName,
                   inventory,
                   price
                );
            });

            //Assert
            Assert.AreEqual(
                $"El producto con ID {productId} fue eliminado anteriormente.",
                ex.Message
            );            
            await ProductRepository.ReceivedWithAnyArgs(1).GetByIdAsync(Arg.Any<object>());
        }

        [TestMethod]
        public async Task DeleteProductAsync_ProductNotExistFailed()
        {
            //Arrange            
            int productId = 1;

            //Act            
            AppException ex = await Assert.ThrowsExceptionAsync<AppException>(async () =>
            {
                await Service.DeleteProductAsync(
                   productId
                );
            });

            //Assert
            Assert.AreEqual(
                $"El producto con ID {productId} no existe.",
                ex.Message
            );
        }

        [TestMethod]
        public async Task DeleteProductAsync_ProductBeforeDeletedFailed()
        {
            //Arrange           
            int productId = 1;

            Product product = ProductBuilder
                .WithProductId(productId)
                .WithIsDeleted(true)
                .Build();

            ProductRepository.GetByIdAsync(productId).ReturnsForAnyArgs(product);

            //Act            
            AppException ex = await Assert.ThrowsExceptionAsync<AppException>(async () =>
            {
                await Service.DeleteProductAsync(
                   productId
                );
            });

            //Assert
            Assert.AreEqual(
                $"El producto con ID {productId} fue eliminado anteriormente.",
                ex.Message
            );
            await ProductRepository.ReceivedWithAnyArgs(1).GetByIdAsync(Arg.Any<object>());
        }
        
        [TestMethod]
        public async Task DeleteProductAsync_Ok()
        {
            //Arrange           
            int productId = 1;

            Product product = ProductBuilder
                .WithProductId(productId)
                .WithIsDeleted(false)
                .Build();

            ProductRepository.GetByIdAsync(productId).ReturnsForAnyArgs(product);
            ProductRepository.UpdateAsync(product).ReturnsForAnyArgs(product);
            //Act            

            await Service.DeleteProductAsync(
                productId
            );
            
            //Assert            
            await ProductRepository.ReceivedWithAnyArgs(1).GetByIdAsync(Arg.Any<object>());
            await ProductRepository.ReceivedWithAnyArgs(1).UpdateAsync(Arg.Any<Product>());
        }        
    }
}
