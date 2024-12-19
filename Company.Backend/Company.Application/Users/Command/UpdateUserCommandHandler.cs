using Company.Application.DTOs;
using Company.Domain.Services;
using Company.Infrastructure;
using MediatR;

namespace Company.Application.Users.Command
{
    public class UpdateUserCommandHandler(
        UserService service
    ) : IRequestHandler<UpdateUserCommand, UsertDto>
    {
        public async Task<UsertDto> Handle(
            UpdateUserCommand command,
            CancellationToken cancellationToken
        )
        {
            User user = await service.UpdateUserAsync(
                command.UserId,
                command.UserName,
                command.Email,
                command.PasswordHash,
                command.Roles
            );

            List<RoleDto> roles = user.Usersinroles
                .Select(role => new RoleDto
                {
                    RoleId = role.Roleid,
                    RolName = role.Role.Rolename
                })
                .ToList();

            return new UsertDto()
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
