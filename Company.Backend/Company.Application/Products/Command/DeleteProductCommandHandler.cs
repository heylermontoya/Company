using Company.Domain.Services;
using MediatR;

namespace Company.Application.Products.Command
{
    public class DeleteProductCommandHandler(
        ProductService service
    ) : IRequestHandler<DeleteProductCommand, Unit>
    {
        public async Task<Unit> Handle(
            DeleteProductCommand command,
            CancellationToken cancellationToken
        )
        {
            await service.DeleteProductAsync(
                command.Id
            );

            return Unit.Value;
        }
    }
}
