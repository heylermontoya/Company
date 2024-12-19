using Company.Application.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Company.Application.Users.Queries
{
    public record GetUserByUserIdQuery(
        [Required] int UserId
    ) : IRequest<UserDto>;
}
