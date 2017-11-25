
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Configuration;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Api.Controllers
{
  public class ExampleController : Base<Example>
  {
    public ExampleController(IOptions<General> config) : base(config)
    {
      // Add any routes that are not basic CRUD.
    }
    [HttpGet]
    public override async Task<IEnumerable<Example>> Get()
    {
      return await _db.Collection.Find(new BsonDocument()).ToListAsync();
    }
  }
}
