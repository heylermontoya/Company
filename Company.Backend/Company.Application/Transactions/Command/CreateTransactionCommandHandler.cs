using Company.Application.DTOs;
using Company.Domain.Services;
using Company.Infrastructure;
using MediatR;

namespace Company.Application.Transactions.Command
{
    public class CreateTransactionCommandHandler(
        TransactionService service
    ) : IRequestHandler<CreateTransactionCommand, TransactionDto>
    {
        public async Task<TransactionDto> Handle(
            CreateTransactionCommand command,
            CancellationToken cancellationToken
        )
        {
            Transaction transaction = await service.CreateTransactionAsync(
                command.ProductId,
                command.UserId,
                command.Quantity
            );

            return new TransactionDto()
            {
                TransactionId = transaction.Transactionid,
                Product = new(){
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
            };
        }
    }
}
