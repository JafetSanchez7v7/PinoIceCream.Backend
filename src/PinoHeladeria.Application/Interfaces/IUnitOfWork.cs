using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable 
    {
        public ICategoryRepository CategoriesI { get; }
        public ICustomersRepository CustomersI { get; }
        public IInventoryRepository InventoryI { get; }
        public IProductRepository ProductsI { get; }
        public ISalesRepository SalesI { get; }
        public ISuppliersRepository SuppliersI { get; }
        public IUsersRepository UsersI { get; }
        public IRoleRepository RolesI { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellation = default);
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
    