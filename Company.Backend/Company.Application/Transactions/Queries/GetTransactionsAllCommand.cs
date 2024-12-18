using MediatR;

namespace Company.Application.Transactions.Queries
{
    public record GetTransactionsAllCommand() : IRequest;
}
