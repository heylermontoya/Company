using Company.Application.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Company.Application.Users.Command
{
    public record CreateUserCommand(
        [Required] string UserName,
        [Required] string Email,
        [Required] string PasswordHash,
        [Required] List<string> Roles
    ) : IRequest<UserDto>;
}
