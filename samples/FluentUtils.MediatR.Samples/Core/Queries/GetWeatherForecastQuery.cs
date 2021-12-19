using FluentUtils.MediatR.Pagination;
using MediatR;

namespace FluentUtils.MediatR.Samples.Core.Queries;

public class GetWeatherForecastQuery : PaginatedRequest<WeatherForecast>
{
    public class Handler : IRequestHandler<GetWeatherForecastQuery, PaginatedResponse<WeatherForecast>>
    {
        private const int Total = 30;

        private static readonly string[] Summaries =
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        /// <inheritdoc />
        public async Task<PaginatedResponse<WeatherForecast>> Handle(GetWeatherForecastQuery request, CancellationToken cancellationToken)
        {
            PaginatedResponse<WeatherForecast> result = new()
            {
                Limit = request.Limit,
                Offset = request.Offset,
                Total = Total
            };

            if (request.Offset >= Total)
                return result;

            int count = request.Limit;

            if (request.Offset + request.Limit > Total)
                count = Total - request.Offset;

            IEnumerable<WeatherForecast> forecasts = Enumerable.Range(request.Offset, count).Select(
                index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                }
            );

            result.Results = forecasts;

            return await Task.FromResult(result);
        }
    }
}
