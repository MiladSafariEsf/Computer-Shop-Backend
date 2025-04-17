using APICH.CORE.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APICH.CORE.interfaces
{
    public interface IReviewService
    {
        public Task<Reviews> GetReviewById(Guid reviewId);
        public Task<List<Reviews>> GetReviewByProductId(Guid productId);
        public Task<int> AddReview(Reviews review);
        public Task<int> EditReview(Reviews review);
        public Task<int> ReviewCountByProductId(Guid ProductId);
        public Task<float> GetAverageRate(Guid Id);
        public Task<int> DeleteReview(Guid Id);
    }
}
