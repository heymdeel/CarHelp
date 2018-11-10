using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHelp.ViewModels
{
    internal class CreatedOrderVM
    {
        [JsonProperty("order_id")]
        public int Id { get; set; }
    }
}
