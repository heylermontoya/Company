using Company.Application.DTOs;
using Company.Domain.Services;
using Company.Infrastructure;
using MediatR;

namespace Company.Application.Products.Command
{
    public class UpdateProductCommandHandler(
        ProductService service
    ) : IRequestHandler<UpdateProductCommand, ProductDto>
    {
        public async Task<ProductDto> Handle(
            UpdateProductCommand command,
            CancellationToken cancellationToken
        )
        {
            Product product = await service.UpdateProductAsync(
                command.ProductId,
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
