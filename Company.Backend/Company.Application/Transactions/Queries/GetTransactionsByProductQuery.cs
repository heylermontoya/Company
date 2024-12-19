using Company.Application.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Company.Application.Transactions.Queries
{
    public record GetTransactionsByProductQuery(
        [Required] int ProductId
    ) : IRequest<List<TransactionDto>>;
}
