using FluentValidation;
using MediatR;
using MyBudgetly.Application.Users.Dto.Models;
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

    public class Handler : IRequestHandler<Message, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserUniquenessChecker _uniquenessChecker;
        private readonly UserApplicationService _userApplicationService;

        public Handler(
            IUserRepository userRepository,
            IUserUniquenessChecker uniquenessChecker,
            UserApplicationService userApplicationService
            )
        {
            _userRepository = userRepository;
            _uniquenessChecker = uniquenessChecker;
            _userApplicationService = userApplicationService;
        }

        public async Task<Guid> Handle(Message message, CancellationToken cancellationToken)
        {
            var userDto = message.UserDto;

            var isUnique = await _uniquenessChecker.IsEmailUniqueAsync(userDto.Email, cancellationToken);
            if (!isUnique)
            {
                throw MyBudgetlyExceptions.GetEmailAlreadyInUseException(userDto.Email);
            }

            var user = _userApplicationService.CreateUser(
                userDto.Email,
                userDto.FirstName,
                userDto.LastName,
                userDto.BackupEmail
                );

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();
            return user.Id;
        }
    }
}
