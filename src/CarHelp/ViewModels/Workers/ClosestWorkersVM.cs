using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHelp.ViewModels
{
    internal class ClosestWorkersVM
    {
        [JsonProperty("worker")]
        public UserProfileVM Worker { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("distance")]
        public double Distance { get; set; }
    }
}
