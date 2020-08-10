using Live_Menu_Point_Of_Sale.Aggregate;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live_Menu_Point_Of_Sale.AggregateFactory
{
    public class ProductsRepository
    {
        private string c_str;

        public ProductsRepository()
        {
            c_str = @"Data Source=./LMdatabase.db;Version=3;";

            CreateTablesIfNotExists();
        }

        public void CreateTablesIfNotExists()
        {
            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS products (" +
                "id TEXT," +
                "category_id TEXT," +
                "name TEXT," +
                "description TEXT,"+
                "`order` INTEGER"+
                ")";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS variations (" +
                "id TEXT," +
                "product_id TEXT," +
                "name TEXT," +
                "cpn INTEGER," +
                "delivery_price REAL," +
                "collection_price REAL," +
                "dine_in_price REAL" +
                ")";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS option_keys (" +
                "id TEXT," +
                "name TEXT" +
                ")";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS option_values (" +
                "id TEXT," +
                "name TEXT," +
                "option_key_id TEXT," +
                "delivery_price REAL," +
                "collection_price REAL," +
                "dine_in_price REAL" +
                ")";
            cmd.ExecuteNonQuery();


            cmd.CommandText = "CREATE TABLE IF NOT EXISTS add_ons (" +
                "id TEXT," +
                "name TEXT," +
                "delivery_price REAL," +
                "collection_price REAL," +
                "dine_in_price REAL" +
                ")";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS categories (" +
                "id TEXT," +
                "name TEXT" +
                ")";
            cmd.ExecuteNonQuery();

            // many to many tables

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS products_option_keys (" +
                "id TEXT," +
                "product_id TEXT," +
                "option_key_id TEXT" +
                ")";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS products_add_ons (" +
                "id TEXT," +
                "product_id TEXT," +
                "add_on_id TEXT" +
                ")";
            cmd.ExecuteNonQuery();


            cmd.CommandText = "CREATE TABLE IF NOT EXISTS saved_variations (" +
                "id TEXT," +
                "name TEXT," +
                "cpn INTEGER," +
                "delivery_price REAL," +
                "collection_price REAL," +
                "dine_in_price REAL" +
                ")";
            cmd.ExecuteNonQuery();

            con.Close();

            

        }


        public void AddProduct(Product product)
        {
            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "INSERT INTO products VALUES(@id, @category_id, @name, @description, @order)";
            cmd.Parameters.Add(new SQLiteParameter("@id", product.Id));
            cmd.Parameters.Add(new SQLiteParameter("@category_id", product.CategoryId));
            cmd.Parameters.Add(new SQLiteParameter("@name", product.Name));
            cmd.Parameters.Add(new SQLiteParameter("@description", product.Description));
            cmd.Parameters.Add(new SQLiteParameter("@order", product.Order));

            cmd.ExecuteNonQuery();

            con.Close();
        }

        public void AddVariation(Variation variation)
        {
            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "INSERT INTO variations VALUES (" +
                "@id," +
                "@product_id," +
                "@name," +
                "@cpn," +
                "@delivery_price," +
                "@collection_price," +
                "@dine_in_price)";

            cmd.Parameters.AddWithValue("@id", variation.Id);
            cmd.Parameters.AddWithValue("@product_id", variation.ProductId);
            cmd.Parameters.AddWithValue("@name", variation.Name);
            cmd.Parameters.AddWithValue("@cpn", variation.Cpn);
            cmd.Parameters.AddWithValue("@delivery_price", variation.DeliveryPrice);
            cmd.Parameters.AddWithValue("@collection_price", variation.CollectionPrice);
            cmd.Parameters.AddWithValue("@dine_in_price", variation.DineInPrice);

            cmd.ExecuteNonQuery();
            
            con.Close();

        }

        public void AddOptionValue(OptionValue optionValue)
        {
            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "INSERT INTO option_values VALUES (" +
                "@id," +
                "@name," +
                "@option_key_id," +
                "@delivery_price," +
                "@collection_price," +
                "@dine_in_price)";

            cmd.Parameters.AddWithValue("@id", optionValue.Id);
            cmd.Parameters.AddWithValue("@name", optionValue.Name);
            cmd.Parameters.AddWithValue("@option_key_id", optionValue.OptionKeyId);
            cmd.Parameters.AddWithValue("@delivery_price", optionValue.DeliveryPrice);
            cmd.Parameters.AddWithValue("@collection_price", optionValue.CollectionPrice);
            cmd.Parameters.AddWithValue("@dine_in_price", optionValue.DineInPrice);

            cmd.ExecuteNonQuery();

            con.Close();
        }

        public void AddOptionKey(OptionKey optionKey)
        {
            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "INSERT INTO option_keys VALUES (" +
                "@id," +
                "@name" +
                ")";

            cmd.Parameters.AddWithValue("@id", optionKey.Id);
            cmd.Parameters.AddWithValue("@name", optionKey.Name);

            cmd.ExecuteNonQuery();

            con.Close();
        }

        public void AddProductOptionKey(Guid productId, Guid optionKeyId)
        {
            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "INSERT INTO products_option_keys VALUES (" +
                "@id," +
                "@product_id," +
                "@option_key_id" +
                ")";
            cmd.Parameters.AddWithValue("@id", Guid.NewGuid());
            cmd.Parameters.AddWithValue("@product_id", productId);
            cmd.Parameters.AddWithValue("@option_key_id", optionKeyId);

            cmd.ExecuteNonQuery();

            con.Close();
        }

        public void RemoveProductOptionKey(Guid productId, Guid optionKeyId)
        {
            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "DELETE FROM products_option_keys WHERE " +
                "product_id = @product_id and" +
                "option_key_id = @option_key_id";

            cmd.Parameters.AddWithValue("@product_id", productId);
            cmd.Parameters.AddWithValue("@option_key_id", optionKeyId);

            cmd.ExecuteNonQuery();

            con.Close();
        }

        public Product GetProduct(int id)
        {
            var products = new List<Product>();

            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "SELECT * FROM products WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                var p = new Product
                {
                    Id = rdr.GetGuid(0),
                    CategoryId = rdr.GetGuid(1),
                    Name = rdr.GetString(2),
                    Description = rdr.GetString(3),
                    Order = rdr.GetInt32(4)
                };

                products.Add(p);
            }

            con.Close();

            return products.FirstOrDefault();
        }

        public void DeleteProduct(Guid id)
        {
            // delete product
            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "DELETE FROM products WHERE " +
                "id = @id";

            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQueryAsync();

            con.Close();

            // delete variatoons

            
            con.Open();

            using var cmd2 = new SQLiteCommand(con);

            cmd2.CommandText = "DELETE FROM variations WHERE " +
                "product_id = @product_id";

            cmd2.Parameters.AddWithValue("@product_id", id);

            cmd2.ExecuteNonQueryAsync();

            con.Close();

            // delete option maps

            con.Open();

            using var cmd3 = new SQLiteCommand(con);

            cmd3.CommandText = "DELETE FROM products_option_keys WHERE " +
                "product_id = @product_id";

            cmd3.Parameters.AddWithValue("@product_id", id);

            cmd3.ExecuteNonQueryAsync();

            con.Close();

        }


        public IEnumerable<Variation> GetVariationsForProduct(Guid productId)
        {
            var variations = new List<Variation>();

            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "SELECT * FROM variations WHERE product_id = @product_id";
            cmd.Parameters.AddWithValue("@product_id", productId);

            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                var v = new Variation
                {
                    Id = rdr.GetGuid(0),
                    ProductId = rdr.GetGuid(1),
                    Name = rdr.GetString(2),
                    Cpn = rdr.GetInt32(3),
                    DeliveryPrice = rdr.GetDouble(4),
                    CollectionPrice = rdr.GetDouble(5),
                    DineInPrice = rdr.GetDouble(6),
                };

                variations.Add(v);
            }

            con.Close();

            return variations;
        }

        public IEnumerable<OptionValue> GetOptionValues(Guid keyId)
        {
            var values = new List<OptionValue>();

            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "SELECT * FROM option_values WHERE option_key_id = @keyId";
            cmd.Parameters.AddWithValue("@keyId", keyId);

            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                var v = new OptionValue
                {
                    Id = rdr.GetGuid(0),
                    Name = rdr.GetString(1),
                    OptionKeyId = rdr.GetGuid(2),
                    DeliveryPrice = rdr.GetDouble(3),
                    CollectionPrice = rdr.GetDouble(4),
                    DineInPrice = rdr.GetDouble(5),
                };

                values.Add(v);
            }

            con.Close();

            return values;
        }



        public IEnumerable<OptionValue> GetAllOptionValues()
        {
            var values = new List<OptionValue>();

            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "SELECT * FROM option_values";

            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                var v = new OptionValue
                {
                    Id = rdr.GetGuid(0),
                    Name = rdr.GetString(1),
                    OptionKeyId = rdr.GetGuid(2),
                    DeliveryPrice = rdr.GetDouble(3),
                    CollectionPrice = rdr.GetDouble(4),
                    DineInPrice = rdr.GetDouble(5),
                };

                values.Add(v);
            }

            con.Close();

            return values;
        }


        public IEnumerable<OptionKey> GetOptionKeysForProduct(Guid productId)
        {
            var options = new List<OptionKey>();
            var optionKeys = new List<ProductOptionKey>();

            using var con = new SQLiteConnection(c_str); 
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "SELECT * FROM products_option_keys WHERE product_id = @product_id";
            cmd.Parameters.AddWithValue("@product_id", productId);

            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                var o = new ProductOptionKey
                {
                    Id = rdr.GetGuid(0),
                    ProductId = rdr.GetGuid(1),
                    OptionKeyId = rdr.GetGuid(2)
                };

                optionKeys.Add(o);
            }


            using var cmd2 = new SQLiteCommand(con);


            // second step
            foreach (var item in optionKeys)
            {
                cmd2.CommandText = "SELECT * FROM option_keys WHERE id = @id";
                cmd2.Parameters.AddWithValue("@id", item.OptionKeyId);

                using SQLiteDataReader rdr2 = cmd2.ExecuteReader();

                while (rdr2.Read())
                {
                    var o = new OptionKey
                    {
                        Id = rdr2.GetGuid(0),
                        Name = rdr2.GetString(1)
                    };

                    options.Add(o);
                }

            }
            


            con.Close();



            return options;
        }

        public IEnumerable<Product> GetProductsForCategory(Guid categoryId)
        {
            var productList = new List<Product>();

            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "SELECT * FROM products WHERE category_id = @category_id";
            cmd.Parameters.AddWithValue("@category_id", categoryId);

            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                var p = new Product
                {
                    Id = rdr.GetGuid(0),
                    CategoryId = rdr.GetGuid(1),
                    Name = rdr.GetString(2),
                    Description = rdr.GetString(3),
                    Order = rdr.GetInt32(4)
                };

                productList.Add(p);
            }

            con.Close();

            return productList;
        }

        public IEnumerable<Category> GetCategories()
        {
            var categories = new List<Category>();

            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "SELECT * FROM categories";

            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                var c = new Category
                {
                    Id = rdr.GetGuid(0),
                    Name = rdr.GetString(1),
                };

                categories.Add(c);
            }

            con.Close();

            return categories;
        }

        public IEnumerable<Product> GetProducts()
        {
            var productList = new List<Product>();

            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "SELECT * FROM products";
            
            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                var p = new Product
                {
                    Id = rdr.GetGuid(0),
                    CategoryId = rdr.GetGuid(1),
                    Name = rdr.GetString(2),
                    Description = rdr.GetString(3),
                    Order = rdr.GetInt32(4)
                };

                productList.Add(p);
            }

            con.Close();

            return productList;
        }

        public async Task AddCategory(Category category)
        {
            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "INSERT INTO categories VALUES (" +
                "@id," +
                "@name)";

            cmd.Parameters.AddWithValue("@id", category.Id);
            cmd.Parameters.AddWithValue("@name", category.Name);
            

            await cmd.ExecuteNonQueryAsync();

            con.Close();
        }

        public async Task SaveAllCategory(List<Category> categories)
        {
            await ClearCategoriesTable();
            foreach (var item in categories)
            {
                await AddCategory(item);
            }
        }

        private async Task ClearCategoriesTable()
        {
            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "DELETE FROM categories";
            
            await cmd.ExecuteNonQueryAsync();

            con.Close();
        }

        public List<OptionKey> GetAllOptionKeys()
        {
            var options = new List<OptionKey>();

            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "SELECT * FROM option_keys";

            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                var o = new OptionKey
                {
                    Id = rdr.GetGuid(0),
                    Name = rdr.SafeGetString(1),
                };

                options.Add(o);
            }
            return options;
        }

        public void ClearOptionKyesTable()
        {
            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "DELETE FROM option_keys";

            cmd.ExecuteNonQuery();

            con.Close();
        }

        public void ClearOptionValuesTable()
        {
            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "DELETE FROM option_values";

            cmd.ExecuteNonQuery();

            con.Close();
        }

        public void ClearSavedVariationsTable()
        {
            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "DELETE FROM saved_variations";

            cmd.ExecuteNonQuery();

            con.Close();
        }

        public void SaveAllSavedVariations(List<Variation> variations)
        {
            ClearSavedVariationsTable();
            foreach (var v in variations)
            {
                AddSingleSavedVariation(v);
            }
        }

        public void AddSingleSavedVariation(Variation variation)
        {
            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "INSERT INTO saved_variations VALUES (" +
                "@id," +
                "@name," +
                "@cpn," +
                "@delivery_price," +
                "@collection_price," +
                "@dine_in_price)";

            cmd.Parameters.AddWithValue("@id", variation.Id);
            cmd.Parameters.AddWithValue("@name", variation.Name);
            cmd.Parameters.AddWithValue("@cpn", variation.Cpn);
            cmd.Parameters.AddWithValue("@delivery_price", variation.DeliveryPrice);
            cmd.Parameters.AddWithValue("@collection_price", variation.CollectionPrice);
            cmd.Parameters.AddWithValue("@dine_in_price", variation.DineInPrice);

            cmd.ExecuteNonQuery();

            con.Close();
        }

        public IEnumerable<Variation> GetAllSavedVariations()
        {
            var variations = new List<Variation>();

            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "SELECT * FROM saved_variations";

            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                var v = new Variation
                {
                    Id = rdr.GetGuid(0),
                    Name = rdr.GetString(1),
                    Cpn = rdr.GetInt32(2),
                    DeliveryPrice = rdr.GetDouble(3),
                    CollectionPrice = rdr.GetDouble(4),
                    DineInPrice = rdr.GetDouble(5),
                };

                variations.Add(v);
            }

            con.Close();

            return variations;
        }
    }
}
