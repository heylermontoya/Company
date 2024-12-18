using MediatR;

namespace Company.Application.Products.Command
{
    public record CreateProductCommand() : IRequest;
}
