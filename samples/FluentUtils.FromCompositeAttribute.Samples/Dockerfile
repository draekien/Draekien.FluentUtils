FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["samples/FluentUtils.FromCompositeAttribute.Samples/FluentUtils.FromCompositeAttribute.Samples.csproj", "FluentUtils.FromCompositeAttribute.Samples/"]
RUN dotnet restore "samples/FluentUtils.FromCompositeAttribute.Samples/FluentUtils.FromCompositeAttribute.Samples.csproj"
COPY . .
WORKDIR "/src/FluentUtils.FromCompositeAttribute.Samples"
RUN dotnet build "FluentUtils.FromCompositeAttribute.Samples.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FluentUtils.FromCompositeAttribute.Samples.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FluentUtils.FromCompositeAttribute.Samples.dll"]
