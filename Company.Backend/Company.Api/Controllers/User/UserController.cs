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
        public async Task CreateUserAsync(
            CreateUserCommand command
        )
        {
            await mediator.Send(command);
        }

        [HttpPut("UpdateUser")]
        public async Task UpdateUserAsync(
            UpdateUserCommand command
        )
        {
            await mediator.Send(command);
        }        
    }
}
