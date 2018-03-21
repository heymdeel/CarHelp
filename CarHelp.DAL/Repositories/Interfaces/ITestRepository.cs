using CarHelp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarHelp.DAL.Repositories
{
    public interface ITestRepository : IRepository<Order>
    {
        void SomeComplicatedMethod();
    }
}
