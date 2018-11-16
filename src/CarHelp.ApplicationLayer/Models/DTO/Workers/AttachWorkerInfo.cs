using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarHelp.AppLayer.Models.DTO
{
    public class AttachWorkerInfo
    {
        [Required, Range(0, int.MaxValue)]
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
