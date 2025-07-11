using FluentValidation;
using MediatR;
using MyBudgetly.Domain.Users.Exceptions;
using MyBudgetly.Domain.Users;

namespace MyBudgetly.Application.Users.Commands;

public static class DeleteUserCommand
{
    public class Message : IRequest<bool>
    {
        public Guid UserId { get; init; }
    }

    public class Validator : AbstractValidator<Message>
    {
        public Validator()
        {
            RuleFor(m => m.UserId).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<Message, bool>
    {
        private readonly IUserRepository _userRepository;

        public Handler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(Message request, CancellationToken cancellationToken)
        {
            _ = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
                ?? throw MyBudgetlyExceptions.GetUserNotFoundException(request.UserId);

            await _userRepository.RemoveAsync(request.UserId, cancellationToken);
            await _userRepository.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
