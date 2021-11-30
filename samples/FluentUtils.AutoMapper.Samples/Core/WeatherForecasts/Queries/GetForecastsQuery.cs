using AutoMapper;
using FluentUtils.AutoMapper.Samples.Core.WeatherForecasts.Contracts;
using FluentUtils.AutoMapper.Samples.Core.WeatherForecasts.Models;
using MediatR;

namespace FluentUtils.AutoMapper.Samples.Core.WeatherForecasts.Queries;

public class GetForecastsQuery : IRequest<IEnumerable<WeatherForecastDto>>
{
    public class Handler : IRequestHandler<GetForecastsQuery, IEnumerable<WeatherForecastDto>>
    {
        private readonly IWeatherForecastService _weatherForecastService;
        private readonly IMapper _mapper;

        public Handler(IWeatherForecastService weatherForecastService, IMapper mapper)
        {
            _weatherForecastService = weatherForecastService;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<WeatherForecastDto>> Handle(GetForecastsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<WeatherForecast> forecasts = await _weatherForecastService.GetCurrentForecastAsync(cancellationToken);
            var result = _mapper.Map<IEnumerable<WeatherForecastDto>>(forecasts);
            return result;
        }
    }
}
