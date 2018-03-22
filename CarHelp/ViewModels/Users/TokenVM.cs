using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarHelp.ViewModels
{
    internal class TokenVM
    {
        [JsonProperty("user_id")]
        public int Id { get; set; } 
        
        [JsonProperty("roles")]
        public string[] Roles { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}
