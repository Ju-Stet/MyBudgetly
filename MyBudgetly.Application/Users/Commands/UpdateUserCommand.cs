using FluentValidation;
using MediatR;
using MyBudgetly.Domain.Users;

namespace MyBudgetly.Application.Users.Commands.UpdateUser;

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

    public class Handler : IRequestHandler<Message, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly UserService _domainService;

        public Handler(IUserRepository userRepository, UserService domainService)
        {
            _userRepository = userRepository;
            _domainService = domainService;
        }

        public async Task<bool> Handle(Message request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
            if (user == null)
                return false;

            user.UpdateName(request.FirstName, request.LastName);
            user.UpdateBackupEmail(request.BackupEmail);

            await _userRepository.UpdateAsync(user, cancellationToken);
            return true;
        }
    }
}