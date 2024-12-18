using MediatR;

namespace Company.Application.Transactions.Command
{
    public record CreateTransactionCommand() : IRequest;
}
