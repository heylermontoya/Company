using Company.Application.Transactions.Command;
using Company.Application.Transactions.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Company.Api.Controllers.Transaction
{   
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController
    {
        private readonly IMediator mediator;

        public TransactionController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("GetTransactionsAll")]
        public async Task GetTransactionsAllAsync()
        {
            await mediator.Send(
                new GetTransactionsAllCommand()
            );
        }

        [HttpGet("GetTransactionsByProduct/{productId}")]
        public async Task GetTransactionsByProductAsync(
            string productId
        )
        {
            await mediator.Send(
                new GetTransactionsByProductCommand(
                    productId
                )
            );
        }
        
        [HttpGet("GetTransactionsByUserID/{userId}")]
        public async Task GetTransactionsByUserIdAsync(
            string userId
        )
        {
            await mediator.Send(
                new GetTransactionsByUserIdCommand(
                    userId
                )
            );
        }

        [HttpPost("RegisterTransaction")]
        public async Task CreateTransactionAsync(
            CreateTransactionCommand command
        )
        {
            await mediator.Send(command);
        }        

        [HttpDelete("DeleteTransaction")]
        public async Task DeleteTransactionAsync(
            DeleteTransactionCommand command
        )
        {
            await mediator.Send(command);
        }
    }
}
