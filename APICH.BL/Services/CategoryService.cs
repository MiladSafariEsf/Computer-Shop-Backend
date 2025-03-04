using APICH.BL.Services.interfaces;
using APICH.CORE.Entity;
using APICH.DAL.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APICH.BL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Categories> repository;

        public CategoryService(IRepository<Categories> repository)
        {
            this.repository = repository;
        }
        public async Task<int> AddCategory(Categories categories)
        {
            return await repository.Add(categories);
        }

        public async Task<int> DeleteCategoryById(Guid Id)
        {
            return await repository.DeleteById(Id);
        }

        public Task<int> EditCategory(Categories categories)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Categories>> GetCategories()
        {
            return await repository.GetAll();
        }

        public async Task<int> GetCategoryCount()
        {
            return await repository.GetTable().CountAsync();
        }

        public async Task<List<Categories>> GetCategoryCountByPageNumber(int PageNumber)
        {
            return await repository.GetTable().OrderBy(a => a.CategoryName).Skip((PageNumber - 1) * 10).Take(10).ToListAsync();
        }
    }
}
