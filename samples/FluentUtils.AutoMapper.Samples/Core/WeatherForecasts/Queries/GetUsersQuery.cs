using AutoMapper;
using FluentUtils.AutoMapper.Samples.Core.WeatherForecasts.Contracts;
using FluentUtils.AutoMapper.Samples.Core.WeatherForecasts.Models;
using MediatR;

namespace FluentUtils.AutoMapper.Samples.Core.WeatherForecasts.Queries;

public class GetUsersQuery : IRequest<List<UserDto>>
{
    public class Handler : IRequestHandler<GetUsersQuery, List<UserDto>>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public Handler(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<List<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            List<User> users = await _userService.ListAsync(cancellationToken);
            return _mapper.Map<List<UserDto>>(users);
        }
    }
}
