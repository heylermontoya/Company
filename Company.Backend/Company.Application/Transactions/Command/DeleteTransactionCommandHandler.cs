using Company.Domain.Services;
using MediatR;

namespace Company.Application.Transactions.Command
{
    public class DeleteTransactionCommandHandler(
        TransactionService service
    ) : IRequestHandler<DeleteTransactionCommand, Unit>
    {
        public async Task<Unit> Handle(
            DeleteTransactionCommand command,
            CancellationToken cancellationToken
        )
        {
            await service.DeleteTransactionAsync(
                command.TransactionId,
                command.UserId
            );

            return Unit.Value;
        }
    }
}
