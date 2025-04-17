using APICH.CORE.interfaces;
using APICH.CORE.Entity;
using APICH.DAL.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace APICH.BL.Services.Classes
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

        public async Task<List<User>> GetAllUser(int PageNumber)
        {
            return await repository.GetTable().Where(a => a.Role != "Owner").Skip((PageNumber - 1) * 20).Take(20).ToListAsync();
        }

        public async Task<User> GetByNumber(string Number)
        {
            return await repository.GetTable().FirstOrDefaultAsync(x => x.Number == Number);
        }

        public async Task<List<User>> SearchUser(string Search , int PageNumber)
        {
            var searchTerms = Search?.Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? new string[] { };
            var query = repository.GetTable();
            foreach (var term in searchTerms)
            {
                query = query.Where(i => i.UserName.Contains(term) || i.Number.Contains(term));
            }
            return await query.Where(a => a.Role != "Owner").Skip((PageNumber - 1) * 20).Take(20).OrderBy(x => x.UserName).ToListAsync();
        }

        public async Task<int> TuggleAdminByNumber(string Number)
        {
            var User = await repository.GetTable().FirstOrDefaultAsync(a => a.Number == Number);
            User.Role = User.Role == "Admin" ? "User" : "Admin";
            return await repository.Update();
        }

        public async Task<int> UpdateUser()
        {
            return await repository.Update();
        }
    }
}
