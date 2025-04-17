using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APICH.CORE.Entity;

namespace APICH.CORE.interfaces
{
    public interface IBannerService
    {
        public Task<Baners> GetBannerById(Guid id); 
        public Task<List<Baners>> GetAllBaners();
        public Task<List<Baners>> GetAllBanersAdmin();
        public Task<int> DeleteBanerById(Guid id);
        public Task<int> EditBaner(Baners baners);
        public Task<int> AddBaner(Baners baner);
        public Task<int> GetBanerCount();
        public Task<int> GetBanerCountAdmin();
    }
}
