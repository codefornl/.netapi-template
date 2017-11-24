
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    public async Task<IEnumerable<T>> Get()
    {
      return await _db.Collection.Find(new BsonDocument()).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<T> Get(int id)
    {
      var filter = new BsonDocument { { _idcol, id } };
      return await _db.Collection.Find(filter).FirstOrDefaultAsync();
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
    public async Task<bool> Put(int id, [FromBody] T document)
    {
      try
      {
        var filter = new BsonDocument { { _idcol, id } };
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
    public async Task<bool> Delete(int id)
    {
      try
      {
        var filter = new BsonDocument { { _idcol, id } };
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
