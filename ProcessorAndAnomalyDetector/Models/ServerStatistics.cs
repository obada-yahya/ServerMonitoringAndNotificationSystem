using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProcessorAndAnomalyDetector.Models;

public class ServerStatistics
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string ServerIdentifier { get; set; }
    public double MemoryUsage { get; set; } // in MB
    public double AvailableMemory { get; set; } // in MB
    public double CpuUsage { get; set; }
    public DateTime Timestamp { get; set; }
    
    public override string ToString()
    {
        return $"{{\n\t\"ServerIdentifier\": \"{ServerIdentifier}\",\n\t\"MemoryUsage\": {MemoryUsage}MB," +
               $"\n\t\"AvailableMemory\": {AvailableMemory}MB,\n\t\"CpuUsage\": {CpuUsage}," +
               $"\n\t\"Timestamp\": \"{Timestamp:yyyy-MM-ddTHH:mm:ssZ}\"\n}}";
    }
}