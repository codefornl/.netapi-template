
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Newtonsoft.Json;

namespace Api.Controllers
{
  [Route("api/[controller]")]
  public abstract class Base<T> : Controller where T : class
  {
    protected readonly Adapters.Mongo<T> _db;

    public Base(IOptions<Configuration.General> config)
    {
      _db = new Adapters.Mongo<T>(config.Value);
    }

    [HttpGet]
    public virtual async Task<IEnumerable<T>> Get()
    {
      return await _db.Collection.Find(new BsonDocument()).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<T> Get(String id)
    {
      FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
      return await _db.Collection.Find(filter).FirstAsync();
    }

    [HttpPost]
    public async Task Post([FromBody] T document)
    {
      try
      {
        await _db.Collection.InsertOneAsync(document);
      }
      catch (Exception ex)
      {
        throw ex;
      }

    }

    [HttpPut("{id}")]
    public async Task<bool> Put(String id, [FromBody] T document)
    {
      try
      {
        FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
        ReplaceOneResult actionResult = await _db.Collection
                                        .ReplaceOneAsync(filter
                                                        , document
                                                        , new UpdateOptions { IsUpsert = true });
        return actionResult.IsAcknowledged
            && actionResult.ModifiedCount > 0;
      }
      catch (Exception ex)
      {
        throw ex;
        
      }
    }

    [HttpDelete("{id}")]
    public async Task<bool> Delete(String id)
    {
      try
      {
        FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
        DeleteResult actionResult = await _db.Collection.DeleteOneAsync(filter);
        return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;

      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
  }
}
