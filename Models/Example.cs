using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Api.Models
{
  public class Example
  {
    [JsonProperty("id")]
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public String Id { get; set; }

    [JsonProperty("name")]
    [BsonElement("Name")]
    public string Name { get; set; }
    [JsonProperty("updated")]
    public DateTime Updated { get; set; } = DateTime.Now;
    [JsonProperty("created")]
    public DateTime Created { get; set; } = DateTime.Now;
    [JsonProperty("deleted")]
    public Boolean Deleted { get; set; } = false;

    [JsonProperty("user")]
    public int UserId { get; set; } = 0;
  }
}