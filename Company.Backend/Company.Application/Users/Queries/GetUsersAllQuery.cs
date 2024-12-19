using Company.Application.DTOs;
using MediatR;

namespace Company.Application.Users.Queries
{
    public record GetUsersAllQuery() : IRequest<List<UserDto>>;
}
