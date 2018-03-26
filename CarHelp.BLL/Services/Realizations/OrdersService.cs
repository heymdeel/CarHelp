using CarHelp.BLL.Model.BusinessModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarHelp.BLL.Services
{
    public class OrdersService : IOrdersService
    {
        public bool ValidateOrderCategory(int categoryId)
        {
            return Enum.IsDefined(typeof(OrdersCategories), categoryId);
        }
    }
}
