using APICH.CORE.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APICH.BL.Services.interfaces
{
    public interface IOrderService
    {
        public Task<int> AddOrder(Orders order);
        public Task<int> DeliverOrderById(Guid Id);
        public Task<int> DeleteOrderById(Guid Id);
        public Task<Orders> GetOrderById(int id);
        public Task<List<Orders>> GetOrderByUserNumber(string UserNumber);
        public Task<int> AddOrderList(List<Orders> orders);
        public Task<List<Orders>> GetAllOrders(int count);
        public Task<int> GetOrderCount();
    }
}
