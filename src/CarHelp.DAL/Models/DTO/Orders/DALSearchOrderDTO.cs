using System;
using System.Collections.Generic;
using System.Text;

namespace CarHelp.DAL.DTO
{
    public class DALSearchOrderDTO
    {
        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public int[] Categories { get; set; }

        public double Distance { get; set; }

        public int? Limit { get; set; }

        public double? PriceFrom { get; set; }

        public double? PriceTo { get; set; }
    }
}
