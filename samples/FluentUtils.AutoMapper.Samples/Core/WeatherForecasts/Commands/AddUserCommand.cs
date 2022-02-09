using AutoMapper;
using FluentUtils.AutoMapper.Samples.Core.WeatherForecasts.Contracts;
using FluentUtils.AutoMapper.Samples.Core.WeatherForecasts.Models;
using MediatR;

namespace FluentUtils.AutoMapper.Samples.Core.WeatherForecasts.Commands;

public class AddUserCommand : IRequest<UserDto>
{
    public UserDto User { get; init; } = null!;

    public class Handler : IRequestHandler<AddUserCommand, UserDto>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public Handler(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<UserDto> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var toAdd = _mapper.Map<User>(request.User);
            User added = await _userService.AddAsync(toAdd, cancellationToken);
            return _mapper.Map<UserDto>(added);
        }
    }
}
