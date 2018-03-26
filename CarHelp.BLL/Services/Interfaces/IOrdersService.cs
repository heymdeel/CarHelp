using System;
using System.Collections.Generic;
using System.Text;

namespace CarHelp.BLL.Services
{
    public interface IOrdersService
    {
        bool ValidateOrderCategory(int categoryId);
    }
}
