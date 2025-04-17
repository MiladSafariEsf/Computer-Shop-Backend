using APICH.CORE.interfaces;
using APICH.CORE.Entity;
using Microsoft.EntityFrameworkCore;

namespace APICH.BL.Services.Classes
{
    public class ProductService : IProductService
    {
        private readonly DAL.Repository.IRepository<Product> repository;

        public ProductService(DAL.Repository.IRepository<Product> repository)
        {
            this.repository = repository;
        }

        public async Task<int> Add(Product product)
        {
            return await repository.Add(product);
        }



        public async Task<int> DeleteById(Guid id)
        {
            return await repository.DeleteById(id);
        }

        public async Task<List<Product>> GetAll(int PageNumber)
        {
            return await repository.GetTable().Where(x => x.Stock > 0).OrderBy(a => a.Price).Skip((PageNumber - 1) * 10).Take(10).ToListAsync();
        }
        public async Task<List<Product>> GetAllAdmin(int PageNumber)
        {
            return await repository.GetTable().OrderBy(a => a.Price).Skip((PageNumber - 1) * 10).Take(10).ToListAsync();
        }
        public async Task<Product> GetAllDataOfProductById(Guid Id)
        {
            return await repository.GetTable().Include(a => a.Reviews.OrderByDescending(a => a.CreateAt)).ThenInclude(a => a.User).FirstOrDefaultAsync(a => a.Id == Id);
        }

        public async Task<Product> GetById(Guid id)
        {
            return await repository.GetById(id);
        }

        public async Task<List<Product>> GetByIds(List<Guid> productIds)
        {
            return await repository.GetTable()
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();
        }
        public async Task<int> GetProductCount()
        {
            return await repository.GetTable().CountAsync();
        }

        public async Task<List<Product>> Search(string search)
        {
            var searchTerms = search?.Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? new string[] { };
            var query = repository.GetTable();
            foreach (var term in searchTerms)
            {
                query = query.Where(i => i.Name.Contains(term) || i.Description.Contains(term));
            }
            var model = await query.Where(a => a.Stock > 0).Take(20).OrderByDescending(a => a.CreateAt).ToListAsync();
            return model;
        }
        public async Task<List<Product>> AdvancedSearch(string? search , int? maxPrice , int? minPrice , Guid? category)
        {
            var searchTerms = search?.Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? new string[] { };
            var query = repository.GetTable();
            if(!string.IsNullOrWhiteSpace(search))
            {
                foreach (var term in searchTerms)
                {
                    query = query.Where(i => i.Name.Contains(term) || i.Description.Contains(term));
                }
            }
            if (category != null)
                query = query.Where(a => a.CategoriesId ==  category);
            if(maxPrice != null)
                query = query.Where(a => a.Price <= maxPrice);
            if(minPrice != null)
                query = query.Where(a => a.Price >= minPrice);
            var model = await query.Where(a => a.Stock > 0).Take(20).OrderByDescending(a => a.CreateAt).ToListAsync();
            return model;
        }

        public async Task<int> Update(Product product)
        {
            var pro = await repository.GetById(product.Id);
            pro.Description = product.Description;
            pro.Price = product.Price;
            pro.Name = product.Name;
            pro.Stock = product.Stock;
            pro.ImageUrl = product.ImageUrl;
            return await repository.Update();
        }

        public async Task<List<Product>> GetProductsById(List<Guid> productIds)
        {
            return await repository.GetTable().Where(a => productIds.Contains(a.Id)).ToListAsync();
        }
    }
}
