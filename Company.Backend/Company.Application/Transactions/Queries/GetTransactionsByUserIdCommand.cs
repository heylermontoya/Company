using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Company.Application.Transactions.Queries
{
    public record GetTransactionsByUserIdCommand(
        [Required] string UserId
    ) : IRequest;
}
