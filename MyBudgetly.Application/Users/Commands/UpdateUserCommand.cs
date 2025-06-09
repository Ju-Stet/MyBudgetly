using FluentValidation;
using MediatR;
using MyBudgetly.Domain.Users;

namespace MyBudgetly.Application.Users.Commands;

public static class UpdateUserCommand
{
    public class Message : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string? BackupEmail { get; set; }
    }
    public class MessageValidator : AbstractValidator<Message>
    {
        public MessageValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.BackupEmail)
                .EmailAddress()
                .When(x => !string.IsNullOrWhiteSpace(x.BackupEmail));
        }
    }

    public class Handler(IUserRepository userRepository) : IRequestHandler<Message, bool>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<bool> Handle(Message message, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(message.Id, cancellationToken);
            if (user == null)
                return false;

            user.UpdateName(message.FirstName, message.LastName);
            user.UpdateBackupEmail(message.BackupEmail);

            await _userRepository.UpdateAsync(user, cancellationToken);
            return true;
        }
    }
}