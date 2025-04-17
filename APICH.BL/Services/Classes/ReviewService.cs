using APICH.CORE.interfaces;
using APICH.CORE.Entity;
using APICH.DAL.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APICH.BL.Services.Classes
{
    public class ReviewService : IReviewService
    {
        private readonly IRepository<Reviews> repository;

        public ReviewService(IRepository<Reviews> repository)
        {
            this.repository = repository;
        }

        public async Task<int> AddReview(Reviews review)
        {
            return await repository.Add(review);
        }

        public async Task<int> DeleteReview(Guid Id)
        {
            return await repository.DeleteById(Id);
        }

        public Task<int> EditReview(Reviews review)
        {
            throw new NotImplementedException();
        }

        public async Task<float> GetAverageRate(Guid Id)
        {
            var query = repository.GetTable().Where(x => x.ProductId == Id);

            return await query.AnyAsync() // بررسی وجود داده
                ? await query.AverageAsync(a => (float?)a.Rating) ?? 0
                : 0;
        }

        public async Task<Reviews> GetReviewById(Guid reviewId)
        {
            return await repository.GetById(reviewId);
        }

        public async Task<List<Reviews>> GetReviewByProductId(Guid productId)
        {
            return await repository.GetTable().Where(x => x.Id == productId).OrderBy(a => a.CreateAt).ToListAsync();
        }

        public async Task<int> ReviewCountByProductId(Guid Id)
        {
            return await repository.GetTable().Where(a => a.ProductId == Id).CountAsync();
        }
    }
}
