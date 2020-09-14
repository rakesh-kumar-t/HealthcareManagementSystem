using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthcareManagementSystem.Models
{
    public class DrugHouse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ManufactureDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int StockLeft { get; set; }
        public double Price { get; set; }
    }
}