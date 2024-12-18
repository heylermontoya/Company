using Company.Application.Products.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Company.Api.Controllers.Product
{   
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController
    {
        private readonly IMediator mediator;

        public ProductController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        
        [HttpPost("CreateProducts")]
        public async Task CreateProductAsync(
            CreateProductCommand command
        )
        {
            await mediator.Send(command);
        }

        [HttpPut("UpdateProducts")]
        public async Task UpdateProductAsync(
            UpdateProductCommand command
        )
        {
            await mediator.Send(command);
        }

        [HttpDelete("DeleteProducts")]
        public async Task DeleteProductAsync(
            DeleteProductCommand command
        )
        {
            await mediator.Send(command);
        }
    }
}
