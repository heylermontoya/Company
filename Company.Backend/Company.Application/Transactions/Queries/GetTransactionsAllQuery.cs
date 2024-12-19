using Company.Application.DTOs;
using MediatR;

namespace Company.Application.Transactions.Queries
{
    public record GetTransactionsAllQuery() : IRequest<List<TransactionDto>>;
}
