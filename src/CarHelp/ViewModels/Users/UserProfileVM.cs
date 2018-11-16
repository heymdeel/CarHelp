using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHelp.ViewModels
{
    internal class UserProfileVM
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("car_number")]
        public string CarNumber { get; set; }

        [JsonProperty("car_model")]
        public string CarModel { get; set; }
    }
}
