using System;
using System.Collections.Generic;
using System.Text;

namespace CarHelp.DAL.DTO
{
    public class DALOrderCreateDTO
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public int WorkerId { get; set; }

        public int ClientId { get; set; }

        public int CategoryId { get; set; }

        public double Price { get; set; }

        public int Rate { get; set; }

        public int StatusId { get; set; }

        public DateTime BeginingTIme { get; set; }
    }
}
