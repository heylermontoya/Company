using Company.Application.DTOs;
using Company.Domain.Services;
using Company.Infrastructure;
using MediatR;

namespace Company.Application.Products.Queries
{
    public class GetProductByProductIdQueryHandler(
        ProductService service
    ) : IRequestHandler<GetProductByProductIdQuery, ProductDto>
    {
        public async Task<ProductDto> Handle(
            GetProductByProductIdQuery query,
            CancellationToken cancellationToken
        )
        {
            Product product = await service.GetProductByProductIdAsync(query.ProductId);

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
