using APICH.CORE.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APICH.BL.Services
{
    public interface ICategoryService
    {
        public Task<Categories> GetCategories();
        public Task<int> AddCategory(Categories categories);
        public Task<int> DeleteCategoryById(Guid Id);
        public Task<int> EditCategory(Categories categories);
    }
}
