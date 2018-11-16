using System;
using System.Collections.Generic;
using System.Text;

namespace CarHelp.DAL.DTO
{
    public class OrderLocationDTO
    {
        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public double Distance { get; set; }
    }

    public class ClientOrderDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string CarNumber { get; set; }

        public string CarModel { get; set; }
    }

    public class ClosestOrderDTO
    {
        public int Id { get; set; }

        public ClientOrderDTO Client { get; set; }

        public OrderLocationDTO Location { get; set; }

        public DateTime BeginningTime { get; set; }

        public string Commentary { get; set; }

        public int Category { get; set; }

        public double? ClientPrice { get; set; }
    }
}
