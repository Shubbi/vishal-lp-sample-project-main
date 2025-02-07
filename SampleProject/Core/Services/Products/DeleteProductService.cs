using BusinessEntities;
using Common;
using Core.Factories;
using Data.Repositories.InMemory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services.Products
{
    [AutoRegister]
    public class DeleteProductService : IDeleteProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductService(IUnitOfWork unitOfWork)
        {
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
