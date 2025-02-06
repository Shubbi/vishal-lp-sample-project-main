using BusinessEntities;
using Common;
using Data.Repositories.InMemory;

namespace Core.Services.Products
{
    [AutoRegister(AutoRegisterTypes.Singleton)]
    public class UpdateProductService : IUpdateProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;            
        }
        public void Update(Product product, string productName, string productDescription, decimal price, int stockQuantity)
        {
            _unitOfWork.Products.Update(product);
            _unitOfWork.Save();
        }
    }
}
