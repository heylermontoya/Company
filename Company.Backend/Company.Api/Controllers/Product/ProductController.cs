using Company.Application.DTOs;
using Company.Application.Products.Command;
using Company.Application.Products.Queries;
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

        [HttpGet("GetProductsAll")]
        public async Task<IActionResult> GetProductsAllAsync()
        {
            List<ProductDto> listProductDto = await mediator.Send(
                new GetProductsAllQuery()
            );

            return new OkObjectResult(listProductDto);
        }

        [HttpGet("GetProductByProductId/{productId}")]
        public async Task<IActionResult> GetProductByProductIdAsync(
            int productId
        )
        {
            ProductDto productDto = await mediator.Send(
                new GetProductByProductIdQuery(
                    productId
                )
            );

            return new OkObjectResult(productDto);
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
