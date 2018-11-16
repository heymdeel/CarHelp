using CarHelp.AppLayer.Models.DTO;
using CarHelp.DAL.DTO;
using CarHelp.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarHelp.AppLayer.Services
{
    public interface IOrdersService
    {
        Task<IEnumerable<ClosestOrderDTO>> FindClosestOrdersAsync(SearchOrderInput searchInput);
        Task<Order> PlaceOrderAsync(CreateOrderInput orderData, int clientId);
        Task RespondToOrderAsync(int orderId, int workerId, WorkerRespondOrderInput workerData);
        Task AttachWorkerToOrderAsync(int clientId, int orderId, AttachWorkerInfo workerInfo);
    }
}
