using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Company.Application.Transactions.Command
{
    public record DeleteTransactionCommand(
        [Required] int Id
    ) : IRequest;
}
