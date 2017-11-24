using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Api.Models
{
  public class Example 
  {
    [JsonIgnore]
    public ObjectId Id { get; set; }
    [JsonProperty("id")]
    [BsonElement("JsonId")]
    public int JsonId { get; set; }
    [BsonElement("Name")]
    public string Name { get; set; }
  }
}