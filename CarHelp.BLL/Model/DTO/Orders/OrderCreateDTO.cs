using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarHelp.BLL.Model.DTO
{
    public class OrderCreateDTO
    {
        [Required, Range(-90, 90)]
        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [Required, Range(-90, 90)]
        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [Required, Range(0, int.MaxValue)]
        [JsonProperty("worker_id")]
        public int WorkerId { get; set; }

        [Required, Range(0, int.MaxValue)]
        [JsonProperty("category_id")]
        public int CategoryId { get; set; }
    }
}
