using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarHelp.AppLayer.Models.DTO
{
    public class SearchOrderInput
    {
        [Required, Range(-180, 180)]
        public double Longitude { get; set; }

        [Required, Range(-90, 90)]
        public double Latitude { get; set; }

        [Required, Range(0, double.MaxValue)]
        public double Distance { get; set; }

        [MinLength(1)]
        public int[] Categories { get; set; }

        public int? Limit { get; set; }
        
        [Range(0, double.MaxValue)]
        public double? PriceFrom { get; set; }

        [Range(0, double.MaxValue)]
        public double? PriceTo { get; set; } 
    }
}
