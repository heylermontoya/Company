using Company.Application.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Company.Application.Transactions.Queries
{
    public record GetTransactionsByUserIdQuery(
        [Required] int UserId
    ) : IRequest<List<TransactionDto>>;
}
