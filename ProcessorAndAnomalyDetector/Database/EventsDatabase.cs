using MongoDB.Driver;
using ProcessorAndAnomalyDetector.Models;
namespace ProcessorAndAnomalyDetector.Database;

public class EventsDatabase : IEventsDatabase
{
    private const string DatabaseName = "Events_Management";
    private const string CollectionName = "Events";
    private readonly string _connectionString;
    private readonly IMongoCollection<ServerStatistics> _collection;
    
    public EventsDatabase(string connectionString)
    {
        _connectionString = connectionString;
        _collection = ConnectToDatabase();
    }

    private IMongoCollection<ServerStatistics> ConnectToDatabase()
    {
        try
        {
            var client = new MongoClient(_connectionString);
            var db = client.GetDatabase(DatabaseName);
            return db.GetCollection<ServerStatistics>(CollectionName);
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to connect to the database");
        }
        return null;
    }
    
    public async Task StoreEvent(ServerStatistics serverStatistics)
    {
        try
        {
            await _collection.InsertOneAsync(serverStatistics);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public async Task<ServerStatistics?> GetRecentEvent(string serverIdentifier)
    {
        try
        {
            const string searchField = "ServerIdentifier";
            var filter = Builders<ServerStatistics>.Filter.Eq(searchField, serverIdentifier);
            return await _collection.Find(filter).Sort(Builders<ServerStatistics>.Sort.Descending(e => e.Timestamp)).FirstOrDefaultAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return null;
    }
}