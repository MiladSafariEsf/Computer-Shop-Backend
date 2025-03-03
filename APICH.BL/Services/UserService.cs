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
    public class UserService : IUserService
    {
        private readonly IRepository<User> repository;

        public UserService(IRepository<User> repository) 
        {
            this.repository = repository;
        }
        public async Task<int> AddUser(User user)
        {
            return await repository.Add(user);
        }

        public async Task<int> DeleteByUserName(string username)
        {
            var user = await repository.GetTable().FirstOrDefaultAsync(x => x.UserName == username);
            await repository.Delete(user);
            return await repository.Update();
        }

        public async Task<User> GetByNumber(string Number)
        {
            return await repository.GetTable().FirstOrDefaultAsync(x => x.Number == Number);
        }

        public async Task<int> UpdateUser(User user)
        {
            var User = await repository.GetById(user.Id);
            User.HashedPassword = user.HashedPassword;
            return await repository.Update(); 
        }
    }
}
