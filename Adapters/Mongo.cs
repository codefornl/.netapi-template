using MongoDB.Driver;

namespace Api.Adapters
{
  public class Mongo<T> where T : class
  {
    private MongoClient _client;
    private IMongoDatabase _db;
    public IMongoCollection<T> Collection { get; private set; }

    public Mongo(Configuration.General config)
    {
      _client = new MongoClient(config.Database.Protocol + "://" + config.Database.Host + ":" + config.Database.Port);
      _db = _client.GetDatabase(config.Database.Name);
      Collection = _db.GetCollection<T>(typeof(T).Name.ToLower());
    }
  }
}