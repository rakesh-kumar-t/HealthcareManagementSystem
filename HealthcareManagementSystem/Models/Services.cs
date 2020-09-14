using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HealthcareManagementSystem.Models
{
    public class Services
    {
        [Key]
        public int id { get; set; }

        [Key]
        public int Pid { get; set; }
        
        [Required]
        public string TestName { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public float Price { get; set; }

        [Required]
        public float TotalAmount{ get; set; }






    }
}