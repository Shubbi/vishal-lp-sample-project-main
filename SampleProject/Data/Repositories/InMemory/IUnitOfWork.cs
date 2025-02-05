using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories.InMemory
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        IOrderRepository Orders { get; }
        void Save();
    }
}
