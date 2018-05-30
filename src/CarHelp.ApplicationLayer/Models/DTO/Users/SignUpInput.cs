using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarHelp.AppLayer.Models.DTO
{
    public class SignUpInput
    {
        [Required, RegularExpression("^[7][0-9]{10}$")]
        public string Phone { get; set; }

        [Required, Range(1000, 9999)]
        [JsonProperty("sms_code")]
        public int SmsCode { get; set; }

        [Required]
        [JsonProperty("profile")]
        public ProfileInput Profile { get; set; }
    }
}
