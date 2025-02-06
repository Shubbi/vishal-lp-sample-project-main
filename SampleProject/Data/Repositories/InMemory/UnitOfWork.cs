using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories.InMemory
{
    //Note - Unit of work is not needed in our case as such
    //But is useful if we use db

    //Registering as singleton because it is in memory
    //For Databases we will make it Transient
    [AutoRegister(AutoRegisterTypes.Singleton)]
    public class UnitOfWork : IUnitOfWork
    {
        public IProductRepository Products { get; }

        public IOrderRepository Orders { get; }

        public IOrderItemRepository OrderItems { get; }

        public ICustomerRepository Customers { get; }

        public UnitOfWork(
            IProductRepository productRepository, 
            IOrderRepository orderRepository,
            IOrderItemRepository orderItemRepository,
            ICustomerRepository customerRepository
            )
        {
            Products = productRepository;
            Orders = orderRepository;
            OrderItems = orderItemRepository;
            Customers = customerRepository;
        }

        public void Save()
        {
            //This is just a dummy implementation and may come handy if we switch to a database
            //with a statement like context.SaveChanges()
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    //Dispose the context
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
