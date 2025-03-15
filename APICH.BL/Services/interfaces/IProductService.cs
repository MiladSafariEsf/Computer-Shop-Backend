using APICH.CORE.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APICH.BL.Services.interfaces
{
    public interface IProductService
    {
        public Task<List<Product>> GetAll(int PageNumber);
        public Task<List<Product>> GetAllAdmin(int PageNumber);
        Task<List<Product>> GetByIds(List<Guid> productIds);
        public Task<Product> GetAllDataOfProductById(Guid Id);
        public Task<Product> GetById(Guid id);
        public Task<List<Product>> Search(string search);
        public Task<List<Product>> AdvancedSearch(string? search, int? maxPrice, int? minPrice,Guid? category);
        public Task<int> Add(Product product);
        public Task<int> Update(Product product);
        public Task<int> DeleteById(Guid id);
        public Task<int> GetProductCount();

    }
}
