using APICH.CORE.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APICH.BL.Services.interfaces
{
    public interface IUserService
    {
        public Task<User> GetByNumber(string Number);
        public Task<int> AddUser(User user);
        public Task<int> DeleteByUserName(string username);
        public Task<List<User>> SearchUser(string Search, int PageNumber);
        public Task<List<User>> GetAllUser(int PageNumber);
        public Task<int> UpdateUser(User user);
        public Task<int> TuggleAdminByNumber(string Number);
    }
}
