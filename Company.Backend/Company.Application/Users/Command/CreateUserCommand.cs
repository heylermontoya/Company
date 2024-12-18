using MediatR;

namespace Company.Application.Users.Command
{
    public record CreateUserCommand() : IRequest;
}
