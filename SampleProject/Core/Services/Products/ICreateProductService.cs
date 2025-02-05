using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services.Products
{
    public interface ICreateProductService
    {
        Product Create(Guid id, string productName, string productDescription, decimal price,int stockQuantity);
    }
}
