using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using MyBudgetly.Application.Users.Dto.Models;
using MyBudgetly.Domain.Common.Exceptions;
using MyBudgetly.Domain.Users;

namespace MyBudgetly.Application.Users.Commands;

public static class UpdateUserCommand
{
    public class Message : IRequest<bool>
    {
        public Guid UserId { get; set; }
        public UpdateUserDto UserDto { get; init; } = null!;
    }
    public class Validator : AbstractValidator<Message>
    {
        public Validator()
        {
            RuleFor(x => x.UserDto.FirstName)
                .MaximumLength(100)
                .When(x => x.UserDto.FirstName is not null);

            RuleFor(x => x.UserDto.LastName)
                .MaximumLength(100)
                .When(x => x.UserDto.LastName is not null);

            RuleFor(x => x.UserDto.BackupEmail)
                .EmailAddress()
                .When(x => x.UserDto.BackupEmail != null);
        }
    }

    public class Handler(
        IUserRepository userRepository,
        UserDomainService userDomainService,
        ILogger<UpdateUserCommand.Handler> logger) : IRequestHandler<Message, bool>
    {
        public async Task<bool> Handle(Message message, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(message.UserId, cancellationToken);
            if (user == null)
            {
                logger.LogWarning("User with ID {UserId} not found", message.UserId);
                return false;
            }

            try
            {
                await userDomainService.UpdateUserAsync(
                    user,
                    message.UserDto.FirstName,
                    message.UserDto.LastName,
                    message.UserDto.BackupEmail,
                    cancellationToken
                );
            }
            catch (DomainException ex)
            {
                logger.LogWarning(ex, "Validation error while updating user with ID {UserId}", message.UserId);
                throw; 
            }

            await userRepository.UpdateAsync(user, cancellationToken);
            await userRepository.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}