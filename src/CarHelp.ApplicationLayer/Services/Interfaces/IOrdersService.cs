using CarHelp.AppLayer.Models.DTO;
using CarHelp.DAL.Entities;
using CarHelp.DAL.Models.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarHelp.AppLayer.Services
{
    public interface IOrdersService
    {
        Task<IEnumerable<ClosestWorkerkDTO>> FindClosestWorkersAsync(ClientCallHelpDTO clientData);
        Task<Order> PlaceOrderAsync(CreateOrderInput orderData, int clientId);
    }
}
