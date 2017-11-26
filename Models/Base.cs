using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Api.Models
{
  [BsonIgnoreExtraElements]
  public class Base
  {
    [JsonProperty("id")]
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public String Id { get; set; }

    [JsonProperty("updated")]
    public DateTime Updated { get; set; } = DateTime.Now;

    [JsonProperty("created")]
    public DateTime Created { get; set; } = DateTime.Now;

   
  }
}
