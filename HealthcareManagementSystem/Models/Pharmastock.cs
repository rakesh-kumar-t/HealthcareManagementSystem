using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HealthcareManagementSystem.Models
{
    public class Pharmastock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PharmId { get; set; }
        [Required]
        public string MedicineName { get; set; }
        [Required]
        public int Stockleft { get; set; }
        [Required]
        public int DrugId { get; set; }//should be foregin key
        [Required]
        public DateTime Expiry { get; set; }
        [Required]
        public DateTime Dateadded { get; set; }
    }
}