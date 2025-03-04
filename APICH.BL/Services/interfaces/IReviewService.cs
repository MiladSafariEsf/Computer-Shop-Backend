using APICH.CORE.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APICH.BL.Services.interfaces
{
    public interface IReviewService
    {
        public Task<List<Reviews>> GetReviewByProductId(Guid productId);
        public Task<int> AddReview(Reviews review);
        public Task<int> EditReview(Reviews review);
        public Task<int> DeleteReview(Guid productId);
    }
}
