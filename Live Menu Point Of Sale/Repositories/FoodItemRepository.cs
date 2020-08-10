using Live_Menu_Point_Of_Sale.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live_Menu_Point_Of_Sale.Repositories
{
    public class FoodItemRepository
    {
        private readonly IMongoCollection<FoodItem> _collection;

        public FoodItemRepository()
        {
            var client = new MongoClient("mongodb+srv://sabbir:sabbir@cluster0-pkxe0.azure.mongodb.net/LiveMenu?retryWrites=true&w=majority");
            var database = client.GetDatabase("LiveMenu");
            _collection = database.GetCollection<FoodItem>("products");

            var filter = Builders<FoodItem>.Filter.Eq("category", "Special Offers");
            var ss = _collection.Find(filter).ToList();
        }

        public List<FoodItem> GetAll()
        {
            return _collection.AsQueryable<FoodItem>().ToList();
        }

        public List<FoodItem> GetByCategory(string category)
        {
            var filter = Builders<FoodItem>.Filter.Eq("category", category);
            return _collection.Find(filter).ToList();
        }


    }
}
