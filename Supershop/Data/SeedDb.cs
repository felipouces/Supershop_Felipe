﻿using Supershop.Data.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Supershop.Data
{
    public class SeedDb
    {

        private readonly DataContext context;
        private DataContext _context;
        private Random _random;

        public SeedDb(DataContext context)
        {
            _context = context;
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            if (!_context.Products.Any())
            {
                AddProduct("IPhone X");
                AddProduct("Magic Mouse");
                AddProduct("IWatch Series 4");
                AddProduct("IPad Mini");

                await _context.SaveChangesAsync();
            }
        }

        private void AddProduct(string name)
        {
            _context.Products.Add(new Product
            {

                Name = name,
                Price = _random.Next(100, 1000),
                IsAvailable = true,
                Stock = _random.Next(1, 100)

            });


        }
    }
}
