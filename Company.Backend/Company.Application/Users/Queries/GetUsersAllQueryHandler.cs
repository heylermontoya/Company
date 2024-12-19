using Company.Application.DTOs;
using Company.Domain.Services;
using Company.Infrastructure;
using MediatR;

namespace Company.Application.Users.Queries
{
    public class GetUsersAllQueryHandler(
        UserService service
    ) : IRequestHandler<GetUsersAllQuery, List<UserDto>>
    {
        public async Task<List<UserDto>> Handle(
            GetUsersAllQuery query,
            CancellationToken cancellationToken
        )
        {
            List<User> users = await service.GetUsersAllAsync();

            List<UserDto> userDtos = users.Select(user => new UserDto
            {
                UserId = user.Userid,
                UserName = user.Username,
                Email = user.Email,
                Password = user.Passwordhash,
                Roles = user.Usersinroles.Select(role => new RoleDto
                {
                    RoleId = role.Roleid,
                    RolName = role.Role.Rolename
                }).ToList()
            }).ToList();

            return userDtos;
        }
    }
}
