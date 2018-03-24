using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarHelp.BLL.Model.DTO
{
    public class UserSignInDTO
    {
        [Required, RegularExpression("^[7][0-9]{10}$")]
        public string Phone { get; set; }

        [Required, Range(1000, 9999)]
        [JsonProperty("sms_code")]
        public int SmsCode { get; set; }
    }
}
