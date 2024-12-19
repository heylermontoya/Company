using Company.Application.DTOs;
using Company.Domain.Services;
using Company.Infrastructure;
using MediatR;

namespace Company.Application.Users.Queries
{
    public class GetUserByUserIdQueryHandler(
        UserService service
    ) : IRequestHandler<GetUserByUserIdQuery, UserDto>
    {
        public async Task<UserDto> Handle(
            GetUserByUserIdQuery query,
            CancellationToken cancellationToken
        )
        {
            User user = await service.GetUserByUserIdAsync(query.UserId);

            List<RoleDto> roles = user.Usersinroles
                .Select(role => new RoleDto
                {
                    RoleId = role.Roleid,
                    RolName = role.Role.Rolename
                })
                .ToList();

            return new UserDto()
            {
                UserId = user.Userid,
                UserName = user.Username,
                Email = user.Email,
                Password = user.Passwordhash,
                Roles = roles
            };
        }
    }
}
