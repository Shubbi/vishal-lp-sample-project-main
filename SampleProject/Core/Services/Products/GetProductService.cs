using BusinessEntities;
using Common;
using Data.Repositories.InMemory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services.Products
{
    //Registering as singleton because it is in memory
    //For Databases we will make it Transient
    [AutoRegister(AutoRegisterTypes.Singleton)]
    public class GetProductService : IGetProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Product GetProduct(Guid id)
        {
            return _unitOfWork.Products.Get(id);
        }
    }
}
