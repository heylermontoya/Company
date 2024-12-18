using MediatR;

namespace Company.Application.Products.Command
{
    public record UpdateProductCommand() : IRequest;
}
