FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

ENV ANOMALY_DETECTION_CONFIG_MEMORY_USAGE_THRESHOLD_PERCENTAGE=0.8
ENV ANOMALY_DETECTION_CONFIG_MEMORY_USAGE_ANOMALY_THRESHOLD_PERCENTAGE=0.4
ENV ANOMALY_DETECTION_CONFIG_CPU_USAGE_THRESHOLD_PERCENTAGE=0.9
ENV ANOMALY_DETECTION_CONFIG_CPU_USAGE_ANOMALY_THRESHOLD_PERCENTAGE=0.5

ENV SIGNALR_CONFIG__SIGNALR_URL="http://signalreventconsumer:5124/Alert"
ENV RABBITMQ_CONNECTION_STRING="amqp://guest:guest@rabbitmq:5672"
ENV MONGODB_CONFIG__CONNECTION_STRING="mongodb://mongo:27017"

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ProcessorAndAnomalyDetector/ProcessorAndAnomalyDetector.csproj", "ProcessorAndAnomalyDetector/"]
RUN dotnet restore "ProcessorAndAnomalyDetector/ProcessorAndAnomalyDetector.csproj"
COPY . .
WORKDIR "/src/ProcessorAndAnomalyDetector"
RUN dotnet build "ProcessorAndAnomalyDetector.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProcessorAndAnomalyDetector.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProcessorAndAnomalyDetector.dll"]
