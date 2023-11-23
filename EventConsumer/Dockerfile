FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

ENV SIGNALR_CONFIG__SIGNALR_URL="http://signalreventconsumer:5124/Alert"

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["EventConsumer/EventConsumer.csproj", "EventConsumer/"]
RUN dotnet restore "EventConsumer/EventConsumer.csproj"
COPY . .
WORKDIR "/src/EventConsumer"
RUN dotnet build "EventConsumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EventConsumer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventConsumer.dll"]
