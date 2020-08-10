using Caliburn.Micro;
using Live_Menu_Point_Of_Sale.BusinessLogics;
using Live_Menu_Point_Of_Sale.DataStructure;
using Live_Menu_Point_Of_Sale.Models;
using Live_Menu_Point_Of_Sale.Models.BusinessModels;
using Live_Menu_Point_Of_Sale.Services;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live_Menu_Point_Of_Sale.AggregateFactory
{
    public class CartRepository
    {
        private string c_str;

        private ProductsService _productsService;

        public CartRepository()
        {
            c_str = @"Data Source=./LMdatabase.db;Version=3;";

            _productsService = new ProductsService();

            CreateTablesIfNotExists();
        }

        public void CreateTablesIfNotExists()
        {
            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS carts (" +
                "id TEXT," +
                "name TEXT," +
                "assigned_number INTEGER," +
                "cart_note TEXT," +
                "address TEXT," +
                "total_price REAL," +
                "discount REAL," +
                "cart_type TEXT" +
                ")";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS cart_items (" +
                "id TEXT," +
                "food_product_id TEXT," +
                "name TEXT," +
                "cart_id TEXT," +
                "dsc TEXT," +
                "note TEXT," +
                "count INTEGER," +
                "category TEXT," +
                "total_price REAL" +
                ")";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS cart_item_selections (" +
                "id TEXT," +
                "cart_item_id TEXT," +
                "selected_variation_id TEXT" +
                ")";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS cart_item_options (" +
                "id TEXT," +
                "cart_item_id TEXT," +
                "option_key_id TEXT," +
                "option_value_id TEXT" +
                ")";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS numbers (" +
                "id INTEGER" +
                ")";
            cmd.ExecuteNonQuery();

            con.Close();
        }

        public IEnumerable<CartItem> GetAllCartItems(Guid cartId)
        {
            var cartItemList = new List<CartItem>();

            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "SELECT * FROM cart_items where cart_id = @cartId";
            cmd.Parameters.AddWithValue("@cartId", cartId);

            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                var ci = new CartItem
                {
                    Id = rdr.GetGuid(0),
                    FoodProduct = _productsService.GetAllProducts().First(x => x.Id == rdr.GetGuid(1)),
                    Name = rdr.SafeGetString(2),
                    Dsc = rdr.SafeGetString(4),
                    Note = rdr.SafeGetString(5),
                    Count= rdr.GetInt32(6),
                    Category= rdr.SafeGetString(7),
                    //TotalPrice = rdr.GetDouble(8),
                };

                // retrive all selected variations and options

                // retrive variations
                var selectedVariation = RetriveSelectedVariation(ci.Id, ci.FoodProduct.Id);
                ci.FoodProduct.SelectedVariation = selectedVariation;

                // retrive options
                var options = RetriveOptions(ci.Id, ci.FoodProduct.Id);

                if(options != null)
                {
                    foreach (var option in options)
                    {
                        var t = ci.FoodProduct.Options.First(x => x.Key.Id == option.First.Id);
                        t.Value.First = option.Second;
                    }
                }

                cartItemList.Add(ci);
            }

            con.Close();


            




            return cartItemList;
        }

        private IEnumerable<Pair<FoodOptionKey, FoodOptionValue>> RetriveOptions(Guid cartItemId, Guid productId)
        {
            var options = new List<Pair<FoodOptionKey, FoodOptionValue>>();

            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "SELECT * FROM cart_item_options where cart_item_id = @cartItemId";
            cmd.Parameters.AddWithValue("@cartItemId", cartItemId);

            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                var op = new
                {
                    Id = rdr.GetGuid(0),
                    CartItemId = rdr.GetGuid(1),
                    OptionKeyId = rdr.GetGuid(2),
                    OptionValueId = rdr.GetGuid(3),
                };

                // get the key

                // get the value

                if(_productsService.GetOptionsForProduct(productId) != null)
                {
                    options.Add(new Pair<FoodOptionKey, FoodOptionValue>()
                    {
                        First = _productsService.GetOptionsForProduct(productId).First(x => x.Key.Id == op.OptionKeyId).Key,
                        Second = _productsService.GetOptionValuesForKey(op.OptionKeyId).First(x => x.Id == op.OptionValueId),
                    });
                }
                else
                {
                    options = null;
                }
                
            }

            con.Close();

            return options;
        }

        private FoodVariation RetriveSelectedVariation(Guid cartItemId, Guid productId)
        {
            var variations = new List<FoodVariation>();

            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "SELECT * FROM cart_item_selections where cart_item_id = @cartItemId";
            cmd.Parameters.AddWithValue("@cartItemId", cartItemId);

            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                var sv = new
                {
                    Id = rdr.GetGuid(0),
                    CartItemId = rdr.GetGuid(1),
                    SelectedVariationId = rdr.GetGuid(2),
                };

                var vari = _productsService.GetVariationsForProduct(productId).FirstOrDefault(x => x.Id == sv.SelectedVariationId);

                variations.Add(vari);
            }

            con.Close();

            return variations.FirstOrDefault();
        }

        public IEnumerable<Cart> GetAllCarts()
        {
            var cartList = new List<Cart>();

            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "SELECT * FROM carts";


            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                var c = new Cart
                {
                    Id = rdr.GetGuid(0),
                    Name = rdr.SafeGetString(1),
                    AssignedNumber = rdr.GetInt32(2),
                    CartNote = rdr.SafeGetString(3),
                    Address = rdr.SafeGetString(4),
                    TotalPrice = rdr.GetDouble(5),
                    Discount = rdr.GetDouble(6),
                    CartType = rdr.SafeGetString(7),
                };
                cartList.Add(c);
            }

            con.Close();

            foreach (var item in cartList)
            {
                item.CartItems = new Caliburn.Micro.BindableCollection<CartItem>(GetAllCartItems(item.Id));
            }

            return cartList;
        }

        public void SaveCarts(List<Cart> carts)
        {
            RemoveAllFromCarts();
            RemoveAllFromCartItems();

            
            foreach (var cart in carts)
            {
                InsertCart(cart);
            }

        }


        public void InsertCart(Cart cart)
        {
            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "INSERT INTO carts values(@Id, @Name, @AssignedNumber, @CartNote, @Address, @TotalPrice, @Discount, @CartType)";
            cmd.Parameters.AddWithValue("@Id", cart.Id);
            cmd.Parameters.AddWithValue("@Name", cart.Name);
            cmd.Parameters.AddWithValue("@AssignedNumber", cart.AssignedNumber);
            cmd.Parameters.AddWithValue("@CartNote", cart.CartNote);
            cmd.Parameters.AddWithValue("@Address", cart.Address);
            cmd.Parameters.AddWithValue("@TotalPrice", cart.TotalPrice);
            cmd.Parameters.AddWithValue("@Discount", cart.Discount);
            cmd.Parameters.AddWithValue("@CartType", cart.CartType);

            cmd.ExecuteNonQuery();

            con.Close();


            foreach (var item in cart.CartItems)
            {
                item.Id = Guid.NewGuid();
                InsertCartItems(item, cart.Id);
            }
        }

        public void InsertCartItems(CartItem cartItem, Guid cartId)
        {
            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "INSERT INTO cart_items values(@Id, @FoodProductId, @Name, @CartId, @Dsc, @Note, @Count, @Category, @TotalPrice)";
            cmd.Parameters.AddWithValue("@Id", cartItem.Id);
            cmd.Parameters.AddWithValue("@FoodProductId", cartItem.FoodProduct.Id);
            cmd.Parameters.AddWithValue("@Name", cartItem.Name);
            cmd.Parameters.AddWithValue("@CartId", cartId);
            cmd.Parameters.AddWithValue("@Dsc", cartItem.Dsc);
            cmd.Parameters.AddWithValue("@Note", cartItem.Note);
            cmd.Parameters.AddWithValue("@Count", cartItem.Count);
            cmd.Parameters.AddWithValue("@Category", cartItem.Category);
            cmd.Parameters.AddWithValue("@TotalPrice", cartItem.TotalPrice);

            cmd.ExecuteNonQuery();

            con.Close();

            // cart Item may have selections.. we will save those

            // save selected variation
            InsertSelectedVariation(cartItem.Id, cartItem.FoodProduct.SelectedVariation);

            // save all the options

            foreach (var optionKey in cartItem.FoodProduct.Options?.Keys ?? new Dictionary<FoodOptionKey, Pair<FoodOptionValue, BindableCollection<FoodOptionValue>>>().Keys)
            {
                InsertSelectedOptions(cartItem.Id, optionKey.Id, cartItem.FoodProduct.Options[optionKey].First.Id);
            }
        }

        private void InsertSelectedOptions(Guid id, Guid keyId, Guid valueId)
        {
            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "INSERT INTO cart_item_options values(@Id, @CartItemId, @OptionKeyId, @OptionValueId)";
            cmd.Parameters.AddWithValue("@Id", Guid.NewGuid());
            cmd.Parameters.AddWithValue("@CartItemId", id);
            cmd.Parameters.AddWithValue("@OptionKeyId", keyId);
            cmd.Parameters.AddWithValue("@OptionValueId", valueId);

            cmd.ExecuteNonQuery();

            con.Close();
        }

        private void InsertSelectedVariation(Guid id, FoodVariation selectedVariation)
        {
            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "INSERT INTO cart_item_selections values(@Id, @CartItemId, @SelectedVariationId)";
            cmd.Parameters.AddWithValue("@Id", Guid.NewGuid());
            cmd.Parameters.AddWithValue("@CartItemId", id);
            cmd.Parameters.AddWithValue("@SelectedVariationId", selectedVariation.Id);

            cmd.ExecuteNonQuery();

            con.Close();
        }

        private void RemoveAllFromCarts()
        {
            
            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "DELETE FROM carts";


            cmd.ExecuteNonQuery();

            con.Close();
        }

        private void RemoveAllFromCartItems()
        {

            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "DELETE FROM cart_items";


            cmd.ExecuteNonQuery();

            con.Close();
        }

        public void SaveNumbers(List<int> numbers)
        {
            ClearNumbers();

            if (!numbers.Any())
            {
                numbers.Add(0);
            }

            foreach (var item in numbers)
            {
                SaveNumber(item);
            }
        }

        private void SaveNumber(int num)
        {
            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "INSERT INTO numbers VALUES(@id)";
            cmd.Parameters.Add(new SQLiteParameter("@id", num));

            cmd.ExecuteNonQuery();

            con.Close();
        }

        public void ClearNumbers()
        {
            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "DELETE FROM numbers";

            cmd.ExecuteNonQuery();

            con.Close();
        }

        public IEnumerable<int> GetAllNumbers()
        {
            var numbers = new List<int>();

            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "SELECT * FROM numbers";


            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                var num = rdr.GetInt32(0);
                numbers.Add(num);
            }

            con.Close();

            return numbers;
        }
    }
}
