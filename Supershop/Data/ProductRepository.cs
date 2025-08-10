using Microsoft.EntityFrameworkCore;
using Supershop.Data.Entities;
using System.Linq;

namespace Supershop.Data
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly DataContext _context;

        public ProductRepository(DataContext context) : base(context) 
            // this (context) goes to parent GenericRepository<Product>
        {
            _context = context;
        }


        // Method that brings all products and users
        public IQueryable GetAllWithUsers()
        {
            return _context.Products.Include(p => p.User);
                // Include the User entity related to the Product
               
        }
    }
}
