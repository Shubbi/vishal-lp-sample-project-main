using BusinessEntities;
using Common;
using Core.Factories;
using Data.Repositories.InMemory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services.Products
{
    [AutoRegister(AutoRegisterTypes.Scope)]
    public class CreateProductService : ICreateProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIdObjectFactory<Product> _productFactory;

        public CreateProductService(IIdObjectFactory<Product> productFactory, IUnitOfWork unitOfWork)
        {
            _productFactory = productFactory;            
            _unitOfWork = unitOfWork;
        }

        public Product Create(string productName, string productDescription, decimal price, int stockQuantity)
        {
            var product = _productFactory.Create(Guid.NewGuid());
            product.Name = productName;
            product.Description = productDescription;
            product.Price = price;
            product.StockQuantity = stockQuantity;
            _unitOfWork.Products.Add(product);
            _unitOfWork.Save();
            return product;
        }
    }
}
