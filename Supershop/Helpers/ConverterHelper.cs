using Supershop.Data.Entities;
using Supershop.Models;
using System.IO;

namespace Supershop.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public Product ToProduct(ProductViewModel model, string path, bool isNew)
        {

            return new Product
            {
                Id = isNew ? 0 : model.Id, // If it's a new product, set Id to 0
                Name = model.Name,
                Price = model.Price,
                ImageUrl = path, // Use the path for the image URL
                LastPurchase = model.LastPurchase,
                LastSale = model.LastSale,
                IsAvailable = model.IsAvailable,
                Stock = model.Stock,
                User = model.User // Assuming User is a property in ProductViewModel
            };

        }

        public ProductViewModel ToProductViewModel(Product product)
        {

            return new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                LastPurchase = product.LastPurchase,
                LastSale = product.LastSale,
                IsAvailable = product.IsAvailable,
                Stock = product.Stock,
                User = product.User // Assuming User is a property in Product
            };

        }
    }
}
