using CarHelp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarHelp.DAL.Repositories
{
    public class TestRepository : L2DBRepository<Order>, ITestRepository
    {
        public void SomeComplicatedMethod()
        {
            Console.WriteLine("calculating some stuff");
        }
    }
}