using APICH.BL.Services.interfaces;
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
    public class OrderService : IOrderService
    {
        private readonly IRepository<Orders> repository;

        public OrderService(IRepository<Orders> repository)
        {
            this.repository = repository;
        }
        public async Task<int> AddOrder(Orders order)
        {
            return await repository.Add(order);
        }

        public async Task<int> AddOrderList(List<Orders> orders)
        {
            foreach (Orders order in orders)
            {
                order.CreateAt = DateTime.Now;
                repository.AddNS(order);
            }
            return await repository.Update();
        }

        public async Task<int> DeleteOrderById(Guid Id)
        {
            return await repository.DeleteById(Id);
        }

        public async Task<int> DeliverOrderById(Guid Id)
        {
            var order = await repository.GetById(Id);
            order.IsDelivered = true;
            return await repository.Update();
        }

        public async Task<List<Orders>> GetAllDeliveredOrders(int PageNumber)
        {
            return await repository.GetTable().Where(x => x.IsDelivered == true).OrderBy(a => a.UserNumber).Skip((PageNumber - 1) * 10).Take(10).Include(a => a.User).Include(a => a.OrderDetails).ThenInclude(a => a.Product).OrderBy(a => a.CreateAt).ToListAsync();
        }

        public async Task<List<Orders>> GetAllOrders(int PageNumber)
        {
            var d = await repository.GetTable().Where(x => x.IsDelivered == false).OrderBy(a => a.UserNumber).Skip((PageNumber - 1) * 10).Take(10).Include(a => a.User).Include(a => a.OrderDetails).ThenInclude(a => a.Product).ToListAsync();
            d.Select(a => a.CreateAt = a.CreateAt.Date);
            return d;
        }

        public Task<int> GetDeliveredOrderCount()
        {
            return repository.GetTable().Where(x => x.IsDelivered == true).CountAsync();
        }

        public async Task<Orders> GetOrderById(Guid id)
        {
            return await repository.GetById(id);
        }

        public Task<Orders> GetOrderById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Orders>> GetOrderByUserNumber(string UserNumber)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetOrderCount()
        {
            return repository.GetTable().Where(x => x.IsDelivered == false).CountAsync();
        }
    }
}
