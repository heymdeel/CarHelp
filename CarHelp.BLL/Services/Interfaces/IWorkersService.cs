﻿using CarHelp.BLL.Model.DTO;
using CarHelp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarHelp.BLL.Services
{
    public interface IWorkersService
    {
        Task<IEnumerable<(double price, UserProfile worker)>> GetClosestWorkersAsync(ClientCallHelpDTO clientData);
    }
}
