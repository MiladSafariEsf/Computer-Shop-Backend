using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APICH.DAL.Repository
{
    public interface IRepository<T>
    {
        public Task<List<T>> GetAll();
        public Task<T> GetById(Guid id);
        public Task<int> Add(T entity);
        public void AddNS(T entity);
        public IQueryable<T> GetTable();
        public Task<int> DeleteById(Guid id);
        public Task<int> Delete(T entity);
        public Task<int> Update();
    }
}
