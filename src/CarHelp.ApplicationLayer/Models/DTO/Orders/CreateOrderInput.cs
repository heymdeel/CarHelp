﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarHelp.AppLayer.Models.DTO
{
    public class CreateOrderInput
    {
        [Required]
        [JsonProperty("location")]
        public LocationDTO Location { get; set; }

        [StringLength(200, MinimumLength = 3)]
        [JsonProperty("commentary")]
        public string Commentary { get; set; }

        [Range(0, double.MaxValue)]
        [JsonProperty("price")]
        public double? ClientPrice { get; set; }

        [Required, Range(0, int.MaxValue)]
        [JsonProperty("category")]
        public int CategoryId { get; set; }
    }
}
