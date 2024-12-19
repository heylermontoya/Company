using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Company.Application.Products.Command
{
    public record DeleteProductCommand(
        [Required] int Id
    ) : IRequest<Unit>;
}
