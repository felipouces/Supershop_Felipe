using Supershop.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Supershop.Data
{
    public interface IOrderRepository : IGenericRepository<Order>
    {

        Task<IQueryable<Order>> GetOrderAsync(string userName); // method to get orders by user name

        Task<IQueryable<OrderDetailsTemp>> GetDetailTempsAsync(string userName); // method to get temporary order details by user name
    }
}
