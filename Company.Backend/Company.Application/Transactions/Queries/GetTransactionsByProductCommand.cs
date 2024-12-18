using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Company.Application.Transactions.Queries
{
    public record GetTransactionsByProductCommand(
        [Required] string ProductId
    ) : IRequest;
}
