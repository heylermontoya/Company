using Company.Application.DTOs;
using Company.Domain.Services;
using Company.Infrastructure;
using MediatR;

namespace Company.Application.Products.Command
{
    public class CreateProductCommandHandler(
        ProductService service
    ) : IRequestHandler<CreateProductCommand, ProductDto>
    {
        public async Task<ProductDto> Handle(
            CreateProductCommand command,
            CancellationToken cancellationToken
        )
        {
            Product product = await service.CreateProductAsync(
                command.ProductName,
                command.Inventory,
                command.Price
            );

            return new ProductDto()
            {
                ProductId = product.Productid,
                ProductName = product.Productname,
                Price = product.Price,
                Inventory = product.Inventory
            };
        }
    }
}
