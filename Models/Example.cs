using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Api.Models
{
  [BsonIgnoreExtraElements]
  public class Example : Base
  {
    [JsonProperty("name")]
    [BsonElement("Name")]
    public string Name { get; set; }

    [JsonProperty("user")]
    public int UserId { get; set; } = 0;
  }
}