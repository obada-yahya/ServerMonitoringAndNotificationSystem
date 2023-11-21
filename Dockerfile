FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

ENV RABBITMQ_CONNECTION_STRING="amqp://guest:guest@rabbitmq:5672"
ENV SERVERSTATISTICSCONFIG_SAMPLINGINTERVALSECONDS=60

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["StatisticsCollector.csproj", "./"]
RUN dotnet restore "StatisticsCollector.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "StatisticsCollector.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StatisticsCollector.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StatisticsCollector.dll"]