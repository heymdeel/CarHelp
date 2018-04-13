using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarHelp.AppLayer.Models.DTO
{
    public class ClientCallHelpDTO
    {
        [Required, Range(-180, 180)]
        public double Longitude { get; set; }

        [Required, Range(-90, 90)]
        public double Latitude { get; set; }

        [Required, Range(0, int.MaxValue)]
        public int CategoryId { get; set; }
    }
}
