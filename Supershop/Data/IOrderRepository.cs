using Supershop.Data.Entities;
using Supershop.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Supershop.Data
{
    public interface IOrderRepository : IGenericRepository<Order>
    {

        Task<IQueryable<Order>> GetOrderAsync(string userName); // method to get orders by user name

        Task<IQueryable<OrderDetailsTemp>> GetDetailTempsAsync(string userName); // method to get temporary order details by user name

        Task AddItemToOrderAsync(AddItemViewModel model, string userName); // method to add an item to the order

        Task ModifyOrderDetailTempQuantityAsync(int id, double quantity); // method to modify the quantity of a temporary order detail

        Task DeleteDetailTempAsync(int id);
    }
}
