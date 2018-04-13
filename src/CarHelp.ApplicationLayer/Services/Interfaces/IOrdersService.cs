using CarHelp.AppLayer.Models.DTO;
using CarHelp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarHelp.AppLayer.Services
{
    public interface IOrdersService
    {
        bool ValidateOrderCategory(int categoryId);
        Task<Order> CreateOrderAsync(OrderCreateDTO orderData, int clientId, WorkerSupportedCategories supportedCategory);
    }
}
