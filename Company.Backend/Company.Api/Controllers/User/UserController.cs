using Company.Application.DTOs;
using Company.Application.Users.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Company.Api.Controllers.User
{   
    [Route("api/[controller]")]
    [ApiController]
    public class UserController
    {
        private readonly IMediator mediator;

        public UserController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUserAsync(
            CreateUserCommand command
        )
        {
            UsertDto usertDto = await mediator.Send(command);

            return new CreatedResult($"User/{usertDto.UserId}", usertDto);
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUserAsync(
            UpdateUserCommand command
        )
        {
            UsertDto usertDto = await mediator.Send(command);
            return new OkObjectResult(usertDto);
        }
    }
}
