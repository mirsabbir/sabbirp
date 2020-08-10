using Caliburn.Micro;
using Live_Menu_Point_Of_Sale.Aggregate;
using Live_Menu_Point_Of_Sale.AggregateFactory;
using Live_Menu_Point_Of_Sale.DataStructure;
using Live_Menu_Point_Of_Sale.Models;
using Live_Menu_Point_Of_Sale.Models.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live_Menu_Point_Of_Sale.Services
{
    // TODO: rename methods to good names
    public class ProductsService
    {
        private ProductsRepository _productsRepo;

        public ProductsService()
        {
            _productsRepo = new ProductsRepository();
        }

        public void AddProduct(FoodProduct foodProduct)
        {
            _productsRepo.AddProduct(new Product
            {
                CategoryId = foodProduct.CategoryId,
                Description = foodProduct.Description,
                Id = foodProduct.Id,
                Name = foodProduct.Name,
                Order = foodProduct.Order,
            });

            foreach (var vari in foodProduct.Variations)
            {
                _productsRepo.AddVariation(new Variation
                {
                    CollectionPrice = vari.CollectionPrice,
                    ProductId = foodProduct.Id,
                    Id = vari.Id,
                    DeliveryPrice = vari.DeliveryPrice,
                    DineInPrice = vari.DineInPrice,
                    Name = vari.Name,
                    Cpn = vari.Cpn,
                });
            }
        }


        public List<FoodProduct> GetAllProducts()
        {
            // produxts ==> food products
            var products = _productsRepo.GetProducts();

            var foodProducts = products.Select(p => new FoodProduct
            {
                Description = p.Description,
                Id = p.Id,
                Name = p.Name,
                Order = p.Order,
                Variations = new BindableCollection<FoodVariation>(GetVariationsForProduct(p.Id)),
                Options = GetOptionsForProduct(p.Id),
                CategoryId = p.CategoryId,
            }).ToList();

            foodProducts = foodProducts.Select(fp =>
            {
                if (fp.Variations.Count() == 1)
                {
                    fp.SelectedVariation = fp.Variations.First();
                }
                return fp;
            }).ToList();

            return foodProducts;
        }

        public List<FoodVariation> GetVariationsForProduct(Guid productId)
        {
            var variations = _productsRepo.GetVariationsForProduct(productId);
            return variations.Select(v => new FoodVariation
            {
                Id = v.Id,
                CollectionPrice = v.CollectionPrice,
                Cpn = v.Cpn,
                DeliveryPrice = v.DeliveryPrice,
                DineInPrice = v.DineInPrice,
                Name = v.Name,
                ProductId = v.ProductId,
            }).ToList();
        }

        public ObservableDictionary<FoodOptionKey, Pair<FoodOptionValue, BindableCollection<FoodOptionValue>>> GetOptionsForProduct(Guid productId)
        {
            var optionKeys = _productsRepo.GetOptionKeysForProduct(productId);

            var res = new ObservableDictionary<FoodOptionKey, Pair<FoodOptionValue, BindableCollection<FoodOptionValue>>>();

            foreach (var item in optionKeys)
            {
                res.Add(new FoodOptionKey() { Id = item.Id, Name = item.Name }
                , new Pair<FoodOptionValue, BindableCollection<FoodOptionValue>>
                {
                    First = null,
                    Second = new BindableCollection<FoodOptionValue>(GetOptionValuesForKey(item.Id)),
                });
            }

            if(optionKeys.Count() == 0)
            {
                return null;
            }

            return res;
        }

        public List<FoodOptionValue> GetOptionValuesForKey(Guid keyId)
        {
            var values = _productsRepo.GetOptionValues(keyId);

            return values.Select(v => new FoodOptionValue() 
            {
                Id = v.Id,
                CollectionPrice = v.CollectionPrice,
                DeliveryPrice = v.DeliveryPrice,
                DineInPrice = v.DineInPrice,
                Name = v.Name,
                OptionKeyId = v.OptionKeyId,
            }).ToList();
        }

        public List<FoodOptionValue> GetAllOptionValues()
        {
            var values = _productsRepo.GetAllOptionValues();

            return values.Select(v => new FoodOptionValue()
            {
                Id = v.Id,
                CollectionPrice = v.CollectionPrice,
                DeliveryPrice = v.DeliveryPrice,
                DineInPrice = v.DineInPrice,
                Name = v.Name,
                OptionKeyId = v.OptionKeyId,
            }).ToList();
        }

        public IEnumerable<FoodProduct> GetProductsByCategory(Guid id)
        {
            // produxts ==> food products
            var products = _productsRepo.GetProductsForCategory(id);

            var foodProducts = products.Select(p => new FoodProduct
            {
                Description = p.Description,
                Id = p.Id,
                Name = p.Name,
                Order = p.Order,
                Variations = new BindableCollection<FoodVariation>(GetVariationsForProduct(p.Id)),
                Options = GetOptionsForProduct(p.Id),
            });

            foodProducts = foodProducts.Select(fp =>
            {
                if (fp.Variations.Count() == 1)
                {
                    fp.SelectedVariation = fp.Variations.First();
                }
                return fp;
            });

            return foodProducts;
        }

        public List<FoodCategory> GetAllCategories()
        {
            var categories = _productsRepo.GetCategories();

            return categories.Select(c => new FoodCategory
            {
                Id = c.Id,
                Name = c.Name,
            }).ToList();
        }

        public async Task SaveAllCategories(List<FoodCategory> foodCategories)
        {
            var cats = foodCategories.Select(x => new Category()
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
            
            await _productsRepo.SaveAllCategory(cats);
        }

        public List<FoodOptionKey> GetOptionKeys()
        {
            var ops = _productsRepo.GetAllOptionKeys();

            return ops.Select(x => new FoodOptionKey
            {
                Name = x.Name,
                Id = x.Id,
            }).ToList();
        }

        public void AddOptionKeys(List<FoodOptionKey> foodOptionKeys)
        {
            _productsRepo.ClearOptionKyesTable();

            foreach (var key in foodOptionKeys)
            {
                _productsRepo.AddOptionKey(new OptionKey
                {
                    Id = key.Id,
                    Name = key.Name,
                });
            }
        }

        public void AddOptionValues(List<FoodOptionValue> foodOptionValues)
        {
            _productsRepo.ClearOptionValuesTable();

            foreach (var val in foodOptionValues)
            {
                _productsRepo.AddOptionValue(new OptionValue
                {
                    Id = val.Id,
                    Name = val.Name,
                    CollectionPrice = val.CollectionPrice,
                    DeliveryPrice = val.DeliveryPrice,
                    DineInPrice = val.DineInPrice,
                    OptionKeyId = val.OptionKeyId
                });
            }
        }

        public void SaveAllSavedVariations(List<FoodVariation> foodItemVariations)
        {
            var variations = foodItemVariations.Select(v =>
            {
                return new Variation
                {
                    CollectionPrice = v.CollectionPrice,
                    Cpn = v.Cpn,
                    DeliveryPrice = v.DeliveryPrice,
                    DineInPrice = v.DineInPrice,
                    Id = v.Id,
                    Name = v.Name
                };
            });
            _productsRepo.SaveAllSavedVariations(variations.ToList());
        }

        public List<FoodVariation> GetAllSavedVariations()
        {
            return _productsRepo.GetAllSavedVariations().Select(x =>
            {
                return new FoodVariation
                {
                    CollectionPrice = x.CollectionPrice,
                    ProductId = x.ProductId,
                    Cpn = x.Cpn,
                    DeliveryPrice = x.DeliveryPrice,
                    DineInPrice = x.DineInPrice,
                    Name = x.Name,
                    Id = x.Id,
                };
            }).ToList();
        }

        public void AssignOptionKeysToProduct(Guid productId, List<Guid> toAssign)
        {
            var assigned = _productsRepo.GetOptionKeysForProduct(productId).Select(x => x.Id).ToList();

            var toRemove = assigned.Except(toAssign).ToList();
            var toAdd = toAssign.Except(assigned).ToList();

            foreach (var key in toRemove)
            {
                _productsRepo.RemoveProductOptionKey(productId, key);
            }

            foreach (var key in toAdd)
            {
                _productsRepo.AddProductOptionKey(productId, key);
            }
        }
    }
}
