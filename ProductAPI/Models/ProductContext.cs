﻿using Microsoft.EntityFrameworkCore;

namespace ProductAPI.Models
{
    public class ProductContext :DbContext
    {
        public DbSet<Product> Products { get; set; }
        public ProductContext(DbContextOptions<ProductContext> options) : base(options) { }    
    }
}
