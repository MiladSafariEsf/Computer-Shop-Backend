using APICH.CORE.Entity;
using APICH.DAL.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APICH.BL.Services
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

        public Task<int> DeleteReview(Guid productId)
        {
            throw new NotImplementedException();
        }

        public Task<int> EditReview(Reviews review)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Reviews>> GetReviewByProductId(Guid productId)
        {
            return await repository.GetTable().Where(x => x.Id == productId).ToListAsync();
        }
    }
}
