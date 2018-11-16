using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHelp.ViewModels
{
    internal class OrderLocationVM
    {
        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("distance")]
        public double Distance { get; set; }
    }

    internal class ClientOrderVM
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("car_number")]
        public string CarNumber { get; set; }

        [JsonProperty("car_model")]
        public string CarModel { get; set; }
    }

    internal class ClosestOrderVM
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("client")]
        public ClientOrderVM Client { get; set; }

        [JsonProperty("location")]
        public OrderLocationVM Location { get; set; }

        [JsonProperty("beginning_time")]
        public DateTime BeginningTime { get; set; }

        [JsonProperty("commentary")]
        public string Commentary { get; set; }

        [JsonProperty("category")]
        public int Category { get; set; }

        [JsonProperty("client_price")]
        public double? ClientPrice { get; set; }
    }
}
