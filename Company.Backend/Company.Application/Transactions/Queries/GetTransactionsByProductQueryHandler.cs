using Company.Application.DTOs;
using Company.Domain.Services;
using Company.Infrastructure;
using MediatR;

namespace Company.Application.Transactions.Queries
{
    public class GetTransactionsByProductQueryHandler(
        TransactionService service
    ) : IRequestHandler<GetTransactionsByProductQuery, List<TransactionDto>>
    {
        public async Task<List<TransactionDto>> Handle(
            GetTransactionsByProductQuery query,
            CancellationToken cancellationToken
        )
        {
            List<Transaction> transactions = await service.GetTransactionsByProductAsync(
                query.ProductId
            );

            List<TransactionDto> transactionDtos = transactions.Select(transaction =>
                new TransactionDto()
                {
                    TransactionId = transaction.Transactionid,
                    Product = new()
                    {
                        ProductId = transaction.Product.Productid,
                        ProductName = transaction.Product.Productname,
                        Inventory = transaction.Product.Inventory,
                        Price = transaction.Product.Price
                    },
                    User = new UserDto()
                    {
                        UserId = transaction.User.Userid,
                        UserName = transaction.User.Username,
                        Email = transaction.User.Email,
                        Password = transaction.User.Passwordhash,
                        Roles = transaction.User.Usersinroles.Select(role => new RoleDto
                        {
                            RoleId = role.Roleid,
                            RolName = role.Role.Rolename
                        }).ToList()
                    },
                    Quantity = transaction.Quantity,
                    TransactionDate = transaction.Transactiondate ?? DateTime.UtcNow
                }                                
            ).ToList();

            return transactionDtos;
        }
    }
}
