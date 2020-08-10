using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live_Menu_Point_Of_Sale.Models
{
    /// <summary>
    /// FoodItem is our Business Object.
    /// </summary>
    [BsonIgnoreExtraElements]
    public class FoodItem
    {
        public FoodItem()
        {

        }
        
        // this ID is not Business ID
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("dsc")]
        public string Dsc { get; set; }

        [BsonElement("variations")]
        public List<FoodItemVariation> Variations { get; set; }

        [BsonElement("category")]
        public string Category { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class FoodItemOption
    {
        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class FoodItemVariation
    {
        [BsonElement("foodId")]
        public int FoodId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("price")]
        public double Price { get; set; }

        [BsonElement("cpn")]
        public int Cpn { get; set; }

        [BsonElement("order")]
        public int Order { get; set; }

        [BsonElement("options")]
        public List<Dictionary<string, List<FoodItemOption>>> Options { get; set; }
    }
}
