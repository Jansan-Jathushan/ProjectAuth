using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Projectauth.Models;

public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string Name { get; set; } = "";
    public string Description { get; set; } = "";

    public string UserId { get; set; } = ""; // ID of the creator
}
