using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Company.Application.Transactions.Command
{
    public record DeleteTransactionCommand(
        [Required] int TransactionId,
        [Required] int UserId
    ) : IRequest<Unit>;
}
