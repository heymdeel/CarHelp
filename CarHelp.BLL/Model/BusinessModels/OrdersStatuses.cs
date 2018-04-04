﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CarHelp.BLL.Model.BusinessModels
{
    internal enum OrdersStatuses
    {
        Awaiting = 0,
        OnTheWay = 1,
        Performing = 2,
        Completed = 3,
        CanceledByWorker = 4,
        CanceledByClient = 5
    }
}
