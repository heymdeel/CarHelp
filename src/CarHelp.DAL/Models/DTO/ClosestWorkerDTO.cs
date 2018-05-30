using CarHelp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using LinqToDB.Mapping;

namespace CarHelp.DAL.Models.DTO
{
    public class ClosestWorkerkDTO
    {
        public double Price { get; set; }

        public double Distance { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Phone { get; set; }
        
        [Column("car_number")]
        public string CarNumber { get; set; }
    }
}
