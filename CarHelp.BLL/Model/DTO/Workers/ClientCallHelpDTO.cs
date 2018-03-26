using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarHelp.BLL.Model.DTO
{
    public class ClientCallHelpDTO
    {
        [Required, Range(-90, 90)]
        public double Latitude { get; set; }

        [Required, Range(-180, 180)]
        public double Longitude { get; set; }

        [Required]
        [Display(Name = "category")]
        public int CategoryId { get; set; }
    }
}
