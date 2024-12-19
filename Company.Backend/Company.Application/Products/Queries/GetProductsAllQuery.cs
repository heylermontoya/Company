using Company.Application.DTOs;
using MediatR;

namespace Company.Application.Products.Queries
{
    public record GetProductsAllQuery() : IRequest<List<ProductDto>>;
}
