using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APICH.CORE.Entity;

namespace APICH.BL.Services.interfaces
{
    public interface IBanerService
    {
        public Task<List<Baners>> GetAllBaners();
        public Task<int> DeleteBanerById(Guid id);
        public Task<int> EditBaner(Baners baners);
        public Task<int> AddBaner(Baners baner);
    }
}
