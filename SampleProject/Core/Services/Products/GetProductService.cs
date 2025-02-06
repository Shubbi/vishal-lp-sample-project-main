using BusinessEntities;
using Common;
using Data.Repositories.InMemory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services.Products
{    
    [AutoRegister(AutoRegisterTypes.Scope)]
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

        public IEnumerable<Product> GetProducts()
        {
            return _unitOfWork.Products.GetAll();
        }
    }
}
