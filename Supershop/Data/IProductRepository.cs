﻿using Supershop.Data.Entities;
using System.Linq;

namespace Supershop.Data
{
    public interface IProductRepository : IGenericRepository<Product>
    {

        // This interface extends the generic repository for Product entities
        public IQueryable GetAllWithUsers();

    }
}
