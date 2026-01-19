using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using PinoHeladeria.Application.Interfaces;
using PinoHeladeria.Domain.Entities;
using PinoHeladeria.Infrastucture.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Infrastucture.AppDbContext
{
    public class MyAppDbContext : DbContext,IUnitOfWork
    {
        public MyAppDbContext(DbContextOptions<MyAppDbContext> options) : base(options)
        {
           
        }
        public DbSet<Categories> Categories => Set<Categories>();
        public DbSet<Suppliers> Suppliers => Set<Suppliers>();
        public DbSet<Products> Products => Set<Products>();
        public DbSet<Inventories> Inventory => Set<Inventories>();
        public DbSet<Customers> Customers => Set<Customers>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Le decimos explícitamente qué columna es la PK
            modelBuilder.Entity<Categories>().HasKey(c => c.CategoryId);
            modelBuilder.Entity<Suppliers>().HasKey(s => s.SupplierId);
            modelBuilder.Entity<Products>().HasKey(p => p.ProductId);
            modelBuilder.Entity<Inventories>().HasKey(p => p.InventoryId);
            modelBuilder.Entity<Customers>().HasKey(cu => cu.CustomerId);

        }


    }
}
