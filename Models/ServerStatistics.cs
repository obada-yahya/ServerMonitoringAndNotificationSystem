namespace Something.Models;

public class ServerStatistics
{
    public string ServerIdentifier { get; init; }
    public double MemoryUsage { get; init; } // in MB
    public double AvailableMemory { get; init; } // in MB
    public double CpuUsage { get; init; }
    public DateTime Timestamp { get; init; }
    
    public override string ToString()
    {
        return $"{{\n\t\"ServerIdentifier\": \"{ServerIdentifier}\",\n\t\"MemoryUsage\": {MemoryUsage},\n\t\"AvailableMemory\": {AvailableMemory},\n\t\"CpuUsage\": {CpuUsage},\n\t\"Timestamp\": \"{Timestamp:yyyy-MM-ddTHH:mm:ssZ}\"\n}}";
    }
}