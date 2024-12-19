using Company.Application.DTOs;
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
        public async Task<IActionResult> CreateProductAsync(
            CreateProductCommand command
        )
        {
            ProductDto productDto = await mediator.Send(command);
            return new CreatedResult($"Product/{productDto.ProductId}", productDto);
        }

        [HttpPut("UpdateProducts")]
        public async Task<IActionResult> UpdateProductAsync(
            UpdateProductCommand command
        )
        {
            ProductDto productDto = await mediator.Send(command);
            return new OkObjectResult(productDto);
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
