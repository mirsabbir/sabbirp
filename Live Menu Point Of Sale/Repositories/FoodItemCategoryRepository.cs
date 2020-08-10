using Live_Menu_Point_Of_Sale.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live_Menu_Point_Of_Sale.Repositories
{
    public class FoodItemCategoryRepository
    {
        private readonly IMongoCollection<FoodItemCategory> _collection;

        public FoodItemCategoryRepository()
        {
            var client = new MongoClient("mongodb+srv://sabbir:sabbir@cluster0-pkxe0.azure.mongodb.net/LiveMenu?retryWrites=true&w=majority");
            var database = client.GetDatabase("LiveMenu");
            _collection = database.GetCollection<FoodItemCategory>("categories");
        }

        public List<FoodItemCategory> GetAll()
        {
            return _collection.AsQueryable<FoodItemCategory>().ToList();
        }
    }
}
