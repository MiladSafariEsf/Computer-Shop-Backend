using APICH.CORE.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APICH.CORE.interfaces
{
    public interface ICategoryService
    {
        public Task<List<Categories>> GetCategories();
        public Task<Categories> GetCategoryById(Guid Id);
        public Task<int> AddCategory(Categories categories);
        public Task<int> DeleteCategoryById(Guid Id);
        public Task<int> EditCategory(Categories categories);
        public Task<int> GetCategoryCount();
        public Task<List<Categories>> GetCategoryCountByPageNumber(int PageNumber);
    }
}
