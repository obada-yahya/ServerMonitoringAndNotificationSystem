FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5124

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["SignalREventConsumerService/SignalREventConsumerService.csproj", "SignalREventConsumerService/"]
RUN dotnet restore "SignalREventConsumerService/SignalREventConsumerService.csproj"
COPY . .
WORKDIR "/src/SignalREventConsumerService"
RUN dotnet build "SignalREventConsumerService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SignalREventConsumerService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SignalREventConsumerService.dll"]
