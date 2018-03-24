using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarHelp.BLL.Model.DTO
{
    public class UserSignUpDTO
    {
        [Required, RegularExpression("^[7][0-9]{10}$")]
        public string Phone { get; set; }

        [Required, Range(1000, 9999)]
        [JsonProperty("sms_code")]
        public int SmsCode { get; set; }

        [Required, StringLength(15, MinimumLength = 3)]
        public string Name { get; set; }

        [Required, StringLength(15, MinimumLength = 3)]
        public string Surname { get; set; }

        [Required, StringLength(15, MinimumLength = 3)]
        [JsonProperty("car_number")]
        public string CarNumber { get; set; }
    }
}
