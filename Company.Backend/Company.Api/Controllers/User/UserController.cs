using Company.Application.DTOs;
using Company.Application.Users.Command;
using Company.Application.Users.Queries;
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

        [HttpGet("GetUsersAll")]
        public async Task<IActionResult> GetUsersAllAsync()
        {
            List<UserDto> listUserDto = await mediator.Send(
                new GetUsersAllQuery()
            );

            return new OkObjectResult(listUserDto);
        }

        [HttpGet("GetUserByUserId/{userId}")]
        public async Task<IActionResult> GetUserByUserIdAsync(
            int userId
        )
        {
            UserDto usertDto = await mediator.Send(
                new GetUserByUserIdQuery(
                    userId
                )
            );

            return new OkObjectResult(usertDto);
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUserAsync(
            CreateUserCommand command
        )
        {
            UserDto usertDto = await mediator.Send(command);

            return new CreatedResult($"User/{usertDto.UserId}", usertDto);
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUserAsync(
            UpdateUserCommand command
        )
        {
            UserDto usertDto = await mediator.Send(command);
            return new OkObjectResult(usertDto);
        }
    }
}
