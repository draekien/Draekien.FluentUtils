FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["samples/FluentUtils.MinimalApis.EndpointDefinitions.Samples/FluentUtils.MinimalApis.EndpointDefinitions.Samples.csproj", "FluentUtils.MinimalApis.EndpointDefinitions.Samples/"]
RUN dotnet restore "samples/FluentUtils.MinimalApis.EndpointDefinitions.Samples/FluentUtils.MinimalApis.EndpointDefinitions.Samples.csproj"
COPY . .
WORKDIR "/src/FluentUtils.MinimalApis.EndpointDefinitions.Samples"
RUN dotnet build "FluentUtils.MinimalApis.EndpointDefinitions.Samples.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FluentUtils.MinimalApis.EndpointDefinitions.Samples.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FluentUtils.MinimalApis.EndpointDefinitions.Samples.dll"]
