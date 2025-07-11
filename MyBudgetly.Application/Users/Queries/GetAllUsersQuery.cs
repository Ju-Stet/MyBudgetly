using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using MyBudgetly.Application.Users.Dto.Mappers;
using MyBudgetly.Application.Users.Dto.Models;
using MyBudgetly.Domain.Users;

namespace MyBudgetly.Application.Users.Queries;

public static class GetAllUsersQuery
{
    public class Message : IRequest<List<UserDto>>
    {
    }

    public class Handler : IRequestHandler<Message, List<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly UserDtoMapper _mapper;

        public Handler(IUserRepository userRepository, UserDtoMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<UserDto>> Handle(Message message, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync(cancellationToken);
            return _mapper.ToDtoList(users);
        }
    }
}
