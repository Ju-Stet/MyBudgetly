using FluentValidation;
using MediatR;
using MyBudgetly.Application.Users.Dto.Models;
using MyBudgetly.Domain.Common.Exceptions;
using MyBudgetly.Domain.Users;

namespace MyBudgetly.Application.Users.Commands;
public static class CreateUserCommand
{
    public class Message : IRequest<Guid>
    {
        public CreateUserDto UserDto { get; init; } = null!;
    }

    public class Validator : AbstractValidator<Message>
    {
        public Validator()
        {
            RuleFor(m => m.UserDto.FirstName).NotEmpty();
            RuleFor(m => m.UserDto.LastName).NotEmpty();
            RuleFor(m => m.UserDto.Email).NotEmpty()
                .EmailAddress();
        }
    }

    public class Handler(
        IUserRepository userRepository,
        UserApplicationService userApplicationService,
        UserDomainService userDomainService
            ) : IRequestHandler<Message, Guid>
    {
        public async Task<Guid> Handle(Message message, CancellationToken cancellationToken)
        {
            var dto = message.UserDto;

            // 1. Checking the primary email
            var canUseEmail = await userDomainService.CanUseEmailAsync(dto.Email, cancellationToken);
            if (!canUseEmail)
            {
                throw MyBudgetlyExceptions.GetEmailAlreadyInUseException(dto.Email);
            }

            // 2. Checking the backup email (if added)
            if (!string.IsNullOrWhiteSpace(dto.BackupEmail))
            {
                var backupUsed = await userDomainService
                    .CanUseEmailAsync(dto.BackupEmail, cancellationToken);

                if (!backupUsed)
                {
                    throw MyBudgetlyExceptions.GetEmailAlreadyInUseException(dto.BackupEmail);
                }

                // 3. Checking that the primary and backup do not match
                if (dto.Email.Equals(dto.BackupEmail, StringComparison.OrdinalIgnoreCase))
                {
                    throw new DomainException("Backup email must differ from the primary email.");
                }
            }

            var user = userApplicationService.CreateUser(
                dto.Email,
                dto.FirstName,
                dto.LastName,
                dto.BackupEmail
            );

            await userRepository.AddAsync(user, cancellationToken);
            await userRepository.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
    }
}
