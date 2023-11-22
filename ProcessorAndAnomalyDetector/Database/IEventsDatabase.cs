using ProcessorAndAnomalyDetector.Models;

namespace ProcessorAndAnomalyDetector.Database;

public interface IEventsDatabase
{
    public Task StoreEvent(ServerStatistics serverStatistics);
    public Task<ServerStatistics?> GetRecentEvent(string serverIdentifier);
}