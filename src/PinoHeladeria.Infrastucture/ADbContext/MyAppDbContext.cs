using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
        private IDbContextTransaction _currentTransaction;

        public MyAppDbContext(DbContextOptions<MyAppDbContext> options) : base(options)
        {
        }
        public DbSet<Categories> Categories => Set<Categories>();
        public DbSet<Suppliers> Suppliers => Set<Suppliers>();
        public DbSet<Products> Products => Set<Products>();
        public DbSet<Inventories> Inventory => Set<Inventories>();
        public DbSet<Customers> Customers => Set<Customers>();
        public DbSet<PurchaseDetails> PurchaseDetails => Set<PurchaseDetails>();
        public DbSet<Purchases> Purchases => Set<Purchases>();
        public DbSet<Sales> Sales => Set<Sales>();
        public DbSet<SalesDetails> SalesDetails => Set<SalesDetails>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Le decimos explícitamente qué columna es la PK
            modelBuilder.Entity<Categories>().HasKey(c => c.CategoryId);
            modelBuilder.Entity<Suppliers>().HasKey(s => s.SupplierId);
            modelBuilder.Entity<Products>().HasKey(p => p.ProductId);
            modelBuilder.Entity<Inventories>().HasKey(p => p.InventoryId);
            modelBuilder.Entity<Customers>().HasKey(cu => cu.CustomerId);
            modelBuilder.Entity<PurchaseDetails>().HasKey(pd => pd.PDetailId);
            modelBuilder.Entity<Purchases>().HasKey(pu => pu.PurchaseId);
            modelBuilder.Entity<SalesDetails>().HasKey(sd=> sd.SaleDetailId);
            modelBuilder.Entity<Sales>().HasKey(s=> s.SaleId);

        }
        public async Task BeginTransactionAsync()
        {
            _currentTransaction = await Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync();

                if (_currentTransaction != null)
                {
                    await _currentTransaction.CommitAsync();
                }
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.RollbackAsync();
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

}

