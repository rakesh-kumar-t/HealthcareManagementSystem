using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HealthcareManagementSystem.Models
{
    public class Pharmacy
    {
        public int PId { get; set; }//from Patient
        public int PatientName { get; set; }//from Patient
        public string Type { get; set; }//Category from Members
        public string MedId { get; set; }
        [Required]
        public string MedName { get; set; }
        [Required]
        public string Quantity { get; set; }
        [Required]
        public double Price { get; set; }

    }
}