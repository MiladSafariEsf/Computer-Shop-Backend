using APICH.BL.Services.interfaces;
using APICH.CORE.Entity;
using APICH.DAL.Repository;
using Microsoft.EntityFrameworkCore;

namespace APICH.BL.Services.Classes
{
    public class BanerService : IBannerService
    {
        private readonly IRepository<Baners> repository;

        public BanerService(IRepository<Baners> repository)
        {
            this.repository = repository;
        }
        public async Task<int> AddBaner(Baners baner)
        {
            return await repository.Add(baner);
        }

        public async Task<int> DeleteBanerById(Guid id)
        {
            return await repository.DeleteById(id);
        }

        public async Task<int> EditBaner(Baners baners)
        {
            var baner = await repository.GetById(baners.Id);
            baner.BanerName = baners.BanerName;
            baner.IsActive = baners.IsActive;
            baner.BanerImageUrl = baners.BanerImageUrl;
            return await repository.Update();
        }

        public async Task<List<Baners>> GetAllBaners()
        {
            return await repository.GetTable().Where(x => x.IsActive == true).OrderBy(a => a.BanerName).ToListAsync();
        }

        public async Task<List<Baners>> GetAllBanersAdmin(int PageNumber)
        {
            return await repository.GetTable().OrderBy(a => a.BanerName).Skip((PageNumber - 1) * 10).Take(10).ToListAsync();
        }

        public Task<Baners> GetBannerById(Guid id)
        {
            return repository.GetById(id);
        }

        public async Task<int> GetBanerCount()
        {
            return await repository.GetTable().Where(x => x.IsActive).CountAsync();
        }

        public async Task<int> GetBanerCountAdmin()
        {
            return await repository.GetTable().CountAsync();
        }
    }
}
