
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Api.Controllers
{
  [Route("api/[controller]")]
  public abstract class Base<T> : Controller where T : class
  {
    protected Configuration.General config;
    private readonly Adapters.Mongo<T> _db;
    protected string _idcol = "Id";

    public Base(IOptions<Configuration.General> dbConfig)
    {
      config = dbConfig.Value;
      _db = new Adapters.Mongo<T>(config);
    }

    [HttpGet]
    public string Get()
    {
      List<T> result = _db.Collection.Find(new BsonDocument()).ToList();
      return JsonConvert.SerializeObject(result);
    }

    [HttpGet("{id}")]
    public string Get(int id)
    {
      var filter = new BsonDocument { { _idcol, id } };
      var document = _db.Collection.Find(filter).First();
      return JsonConvert.SerializeObject(document);
    }

    [HttpPost]
    public string Post([FromBody] T document)
    {
      _db.Collection.InsertOne(document);
      return JsonConvert.SerializeObject(document);
    }

    [HttpPut("{id}")]
    public async void Put(int id, [FromBody] T document)
    {
      var filter = new BsonDocument { { _idcol, id } };
      await _db.Collection.ReplaceOneAsync(filter, document);
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
      var filter = new BsonDocument { { _idcol, id } };
      var operation = _db.Collection.DeleteOne(filter);
    }
  }
}
