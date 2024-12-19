using Company.Application.DTOs;
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
        public async Task<IActionResult> GetTransactionsAllAsync()
        {
            List<TransactionDto> transactionsDto = await mediator.Send(
                new GetTransactionsAllQuery()
            );

            return new OkObjectResult(transactionsDto);
        }

        [HttpGet("GetTransactionsByProduct/{productId}")]
        public async Task<IActionResult> GetTransactionsByProductAsync(
            int productId
        )
        {
            List<TransactionDto> transactionsDto = await mediator.Send(
                new GetTransactionsByProductQuery(
                    productId
                )
            );

            return new OkObjectResult(transactionsDto);
        }

        [HttpGet("GetTransactionsByUserID/{userId}")]
        public async Task<IActionResult> GetTransactionsByUserIdAsync(
            int userId
        )
        {
            List<TransactionDto> transactionsDto = await mediator.Send(
                new GetTransactionsByUserIdQuery(
                    userId
                )
            );

            return new OkObjectResult(transactionsDto);
        }

        [HttpPost("RegisterTransaction")]
        public async Task<IActionResult> CreateTransactionAsync(
            CreateTransactionCommand command
        )
        {
            TransactionDto transactionDto = await mediator.Send(command);
            return new CreatedResult($"User/{transactionDto.TransactionId}", transactionDto);
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
