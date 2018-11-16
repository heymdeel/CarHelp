using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarHelp.AppLayer.Models.DTO
{
    public class WorkerRespondOrderInput
    {
        [Required]
        [JsonProperty("location")]
        public LocationDTO Location { get; set; }

        [Range(0, double.MaxValue)]
        [JsonProperty("price")]
        public double Price { get; set; }

        [StringLength(200, MinimumLength = 3)]
        [JsonProperty("commentary")]
        public string Commentary { get; set; }
    }
}
