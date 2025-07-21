using Supershop.Data.Entities;

namespace Supershop.Data
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
         public ProductRepository(DataContext context) : base(context) // this (context) goes to parent GenericRepository<Product>
        {
        }
    }
}
