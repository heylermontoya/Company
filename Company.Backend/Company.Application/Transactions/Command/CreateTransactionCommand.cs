using Company.Application.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Company.Application.Transactions.Command
{
    public record CreateTransactionCommand(
        [Required] int ProductId,
        [Required] int UserId,
        [Required] int Quantity
    ) : IRequest<TransactionDto>;
}
