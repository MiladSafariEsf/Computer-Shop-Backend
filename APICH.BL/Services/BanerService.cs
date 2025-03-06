using APICH.BL.Services.interfaces;
using APICH.CORE.Entity;
using APICH.DAL.Repository;

namespace APICH.BL.Services
{
    public class BanerService : IBanerService
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
            baner.BanerImageUrl = baners.BanerImageUrl;
            return await repository.Update();
        }

        public async Task<List<Baners>> GetAllBaners()
        {
            return await repository.GetAll();
        }
    }
}
