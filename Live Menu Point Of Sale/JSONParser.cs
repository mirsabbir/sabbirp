using Live_Menu_Point_Of_Sale.AggregateFactory;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live_Menu_Point_Of_Sale
{
    public class JSONParser
    {
        private ProductsRepository _productsRepository;
        private string _text;

        public JSONParser(string text)
        {
            _text = text;
            _productsRepository = new ProductsRepository();
        }

        public void ParseAndUpload()
        {
            var products = JObject.Parse(_text);

            foreach (var product in products)
            {
                var categoryName = product.Key;

                var cat = new Aggregate.Category
                {
                    Id = Guid.NewGuid(),
                    Name = categoryName,
                };

                _productsRepository.AddCategory(cat) ;

                

                var foodItems = JArray.Parse(product.Value.ToString());

                foreach (var foodItem in foodItems)
                {
                    //{
                    //    id: "sdcd",
                    //    price: "50"
                    //}

                    var foodObject = JObject.Parse(foodItem.ToString());

                    var name = foodObject["name"].ToString();
                    var dsc = foodObject["dsc"].ToString();
                    var price = foodObject["price"]?.ToString();
                    var cpn = foodObject["cpn"]?.ToString();
                    var vari = foodObject["vari"];
                    var options = foodObject["options"];

                    //////
                    /// Insert
                    ///
                    var p = new Aggregate.Product
                    {
                        CategoryId = cat.Id,
                        Id = Guid.NewGuid(),
                        Name = name,
                        Description = dsc,
                        Order = 1
                    };
                    _productsRepository.AddProduct(p);


                    if (vari != null)
                    {
                        // variation product
                        var variations = JArray.Parse(vari.ToString());
                        foreach (var variation in variations)
                        {
                            var v = JObject.Parse(variation.ToString());

                            var vname = v["name"].ToString();
                            var vprice = v["price"].ToString();
                            var vcpn = v["cpn"].ToString();

                            var vv = new Aggregate.Variation
                            {
                                CollectionPrice = Convert.ToDouble(vprice),
                                DineInPrice = Convert.ToDouble(vprice),
                                ProductId = p.Id,
                                Id = Guid.NewGuid(),
                                Cpn = Convert.ToInt32(vcpn),
                                DeliveryPrice = Convert.ToDouble(vprice),
                                Name = vname,
                                Order = 1,
                            };

                            _productsRepository.AddVariation(vv);

                        }
                    }
                    else
                    {
                        // the default one variation

                        var vv = new Aggregate.Variation
                        {
                            CollectionPrice = Convert.ToDouble(price),
                            DineInPrice = Convert.ToDouble(price),
                            ProductId = p.Id,
                            Id = Guid.NewGuid(),
                            Cpn = Convert.ToInt32(cpn),
                            DeliveryPrice = Convert.ToDouble(price),
                            Name = "",
                            Order = 1,
                        };

                        _productsRepository.AddVariation(vv);
                    }

                    if (options != null)
                    {
                        var ops = JArray.Parse(options.ToString());

                        foreach (var op in ops)
                        {
                            var option = JObject.Parse(op.ToString());

                            var optionKeyValue = option.First;

                            string key = JObject.Parse(option.ToString()).Properties().First().Name;

                            var opk = new Aggregate.OptionKey
                            {
                                Id = Guid.NewGuid(),
                                Name = key,
                            };

                            _productsRepository.AddOptionKey(opk);

                            var values = JArray.Parse(optionKeyValue.First.ToString());


                            foreach (var value in values)
                            {
                                var val = JObject.Parse(value.ToString());
                                var valId = val["id"].ToString();
                                var valName = val["name"].ToString();

                                var opv = new Aggregate.OptionValue
                                {
                                    Id = Guid.NewGuid(),
                                    Name = valName,
                                    OptionKeyId = opk.Id,
                                    CollectionPrice = 0,
                                    DeliveryPrice = 0,
                                    DineInPrice = 0,
                                };

                                _productsRepository.AddOptionValue(opv);

                                _productsRepository.AddProductOptionKey(p.Id, opk.Id);
                            }

                        }
                    }

                }

            }
        }
    }
}
