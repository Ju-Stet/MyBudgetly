using FluentValidation;
using MediatR;
using MyBudgetly.Application.Users.Dto.Mappers;
using MyBudgetly.Application.Users.Dto.Models;
using MyBudgetly.Domain.Users;
using MyBudgetly.Domain.Users.Exceptions;

namespace MyBudgetly.Application.Users.Queries;

public static class GetUserQuery
{
    public class Message : IRequest<UserDto>
    {
        public Guid UserId { get; init; }
    }

    public class MessageValidator : AbstractValidator<Message>
    {
        public MessageValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }

    public class Handler(
        IUserRepository userRepository,
        UserDtoMapper userDtoMapper
            ) : IRequestHandler<Message, UserDto>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly UserDtoMapper _userDtoMapper = userDtoMapper;

        public async Task<UserDto> Handle(Message message, CancellationToken cancellationToken)
        {
            User user;
            try
            {
                user = await _userRepository.GetByIdAsync(message.UserId, cancellationToken);
            }
            catch (UserNotFoundException ex)
            {
                throw MyBudgetlyExceptions.GetUserNotFoundException(ex.UserId);
            }

            return _userDtoMapper.ToDto(user);
        }
    }
}