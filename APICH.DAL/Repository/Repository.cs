using APICH.CORE.Entity;
using APICH.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace APICH.DAL.Repository
{
    public class Repository<T> : IRepository<T> 
        where T : BaseEntity
    {
        private readonly APICH_DbContext db;
        private DbSet<T> entities;
        public Repository(APICH_DbContext db) 
        {
            this.db = db;
            entities = db.Set<T>();
        }
        public async Task<int> Add(T entity)
        {
            entities.Add(entity);
            return await db.SaveChangesAsync();
        }
        public void AddNS(T entity)
        {
            entities.Add(entity);
        }
        public async Task<int> Delete(T entity)
        {
            entities.Remove(entity);
            return await db.SaveChangesAsync();
        }

        public async Task<int> DeleteById(Guid id)
        {
            entities.Remove(await entities.FindAsync(id));
            return await db.SaveChangesAsync();
        }

        public async Task<List<T>> GetAll()
        {
            return await entities.ToListAsync();
        }

        public async Task<T> GetById(Guid id)
        {
            return await entities.FindAsync(id);
        }

        public IQueryable<T> GetTable()
        {
            return entities.AsQueryable();
        }

        public async Task<int> Update()
        {
            return await db.SaveChangesAsync();
        }
    }
}
