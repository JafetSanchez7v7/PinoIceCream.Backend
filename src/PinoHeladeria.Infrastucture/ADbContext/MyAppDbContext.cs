using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using PinoHeladeria.Application.Interfaces;
using PinoHeladeria.Domain.Entities;
using PinoHeladeria.Infrastucture.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Infrastucture.AppDbContext
{
    public class MyAppDbContext : DbContext,IUnitOfWork
    {
        private IDbContextTransaction _currentTransaction;
        //inyecciones de repositorios para poder llamar desde IUnitofWork y no llenar constructores
        /*
         mas o menos en vez de hacer esto
        var validCategory = await _catRepo.FindAsync(dto.CategoryId);
        y tener un mar de constructores hacer esto
        var valid category = await _uow.Categories.FindAsync(dto.CategoryId);
        asi solo inyectamos IUnitOfWork en mis servicios.
        ese es el porque de estas propiedades de inyeccion
         */
        private IMemoryCache _cache;
        private ICategoryRepository _cat;
        private ICustomersRepository _cus;
        private IInventoryRepository _inv;
        private IProductRepository _prod;
        private ISalesRepository _sales;
        private IUsersRepository _users;
        private IRoleRepository _roles;
        private ISuppliersRepository _suppliers;



        public MyAppDbContext(DbContextOptions<MyAppDbContext> options, IMemoryCache cache) : base(options)
        {
            _cache = cache;
        }
        public DbSet<Categories> Categories => Set<Categories>();
        public DbSet<Suppliers> Suppliers => Set<Suppliers>();
        public DbSet<Products> Products => Set<Products>();
        public DbSet<Inventories> Inventories => Set<Inventories>();
        public DbSet<Customers> Customers => Set<Customers>();
        public DbSet<PurchaseDetails> PurchaseDetails => Set<PurchaseDetails>();
        public DbSet<Purchases> Purchases => Set<Purchases>();
        public DbSet<Sales> Sales => Set<Sales>();
        public DbSet<SalesDetails> SalesDetails => Set<SalesDetails>();
        public DbSet<SsUsers> SsUsers => Set<SsUsers>();
        public DbSet<Roles> Roles => Set<Roles>();
        // invocacuon
        public ICategoryRepository CategoriesI => _cat ?? new CategoryRepository(this,_cache);
        public ICustomersRepository CustomersI => _cus ?? new CustomersRepository(this, _cache);
        public IInventoryRepository InventoryI => _inv ?? new InventoryRepository(this, _cache);
        public IProductRepository ProductsI => _prod ?? new ProductRepository(this);
        public ISalesRepository SalesI => _sales ?? new SalesRepository(this, _cache);
        public IUsersRepository UsersI => _users ?? new UsersRepository(this);
        public IRoleRepository RolesI => _roles ?? new RoleRepository(this);
        public ISuppliersRepository SuppliersI => _suppliers ?? new SuppliersRepository(this);




        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // aqui especifico a efc cual es la pk
            modelBuilder.Entity<Categories>().HasKey(c => c.CategoryId);
            modelBuilder.Entity<Suppliers>().HasKey(s => s.SupplierId);
            modelBuilder.Entity<Products>().HasKey(p => p.ProductId);
            modelBuilder.Entity<Inventories>().HasKey(p => p.InventoryId);
            modelBuilder.Entity<Customers>().ToTable(tb=>tb.HasTrigger("trg_ProtectGeneralCustomer")).HasKey(cu => cu.CustomerId);
            modelBuilder.Entity<PurchaseDetails>().HasKey(pd => pd.PDetailId);
            modelBuilder.Entity<Purchases>().HasKey(pu => pu.PurchaseId);
            modelBuilder.Entity<SalesDetails>().HasKey(sd=> sd.SaleDetailId);
            modelBuilder.Entity<Sales>().HasKey(s=> s.SaleId);
            modelBuilder.Entity<SsUsers>().ToTable(tb=>tb.HasTrigger("trg_Users_PreventModifyAdmin")).HasKey(u => u.UserId);
            modelBuilder.Entity<Roles>().ToTable(tb=>tb.HasTrigger("trg_Roles_PreventModifyRoleId1")).HasKey(u => u.RoleId);


        }
        // Persistencias de operaciones Transaccionales
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

