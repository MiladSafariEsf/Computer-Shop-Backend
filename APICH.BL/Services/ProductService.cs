using APICH.CORE.Entity;
using Microsoft.EntityFrameworkCore;

namespace APICH.BL.Services
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
            return await repository.GetTable().OrderBy(a => a.Price).Skip((PageNumber - 1) * 10).Take(10).ToListAsync();
        }
        public async Task<Product> GetAllDataOfProductById(Guid Id)
        {
            return await repository.GetTable().Include(a => a.Reviews).FirstOrDefaultAsync(a => a.Id == Id);
        }

        public async Task<Product> GetById(Guid id)
        {
            return await repository.GetById(id);
        }

        public async Task<List<Product>> GetByIds(List<Guid> productIds)
        {
            var query = repository.GetTable()
                .Where(p => productIds.Contains(p.Id));

            Console.WriteLine(query.ToQueryString());


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
            var model = await query.Take(10).ToListAsync();
            return model;
        }

        public async Task<int> Update(Product product)
        {
            var pro = await repository.GetById(product.Id);
            pro.Description = product.Description;
            pro.Price = product.Price;
            pro.Name = product.Name;
            pro.ImageUrl = product.ImageUrl;
            return await repository.Update();
        }
    }
}
