using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarHelp.AppLayer.Models.DTO
{
    public class ProfileInput
    {
        [Required, StringLength(15, MinimumLength = 3)]
        public string Name { get; set; }

        [Required, StringLength(15, MinimumLength = 3)]
        public string Surname { get; set; }

        [Required, StringLength(15, MinimumLength = 3)]
        [JsonProperty("car_number")]
        public string CarNumber { get; set; }

        [Required, StringLength(50, MinimumLength = 3)]
        [JsonProperty("car_model")]
        public string CarModel { get; set; }
    }
}
