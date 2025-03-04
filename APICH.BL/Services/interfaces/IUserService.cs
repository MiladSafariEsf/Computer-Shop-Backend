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
        public Task<int> UpdateUser(User user);
    }
}
