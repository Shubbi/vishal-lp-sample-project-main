using BusinessEntities;
using Common;
using Core.Factories;
using Data.Repositories.InMemory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services.Products
{
    [AutoRegister(AutoRegisterTypes.Singleton)]
    public class DeleteProductService : IDeleteProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIdObjectFactory<Product> _productFactory;

        public DeleteProductService(IIdObjectFactory<Product> productFactory, IUnitOfWork unitOfWork)
        {
            _productFactory = productFactory;
            _unitOfWork = unitOfWork;
        }

        public void Delete(Product product)
        {
            _unitOfWork.Products.Delete(product.Id);
            _unitOfWork.Save();
        }

        public void DeleteAll()
        {
            _unitOfWork.Products.DeleteAll();
            _unitOfWork.Save();
        }
    }
}
