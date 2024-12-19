using Company.Application.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Company.Application.Products.Queries
{
    public record GetProductByProductIdQuery(
        [Required] int ProductId
    ) : IRequest<ProductDto>;
}
