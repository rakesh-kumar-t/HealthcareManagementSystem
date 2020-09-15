using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthcareManagementSystem.Models
{
    public class Pharmacy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id{get;set;}
        public int PId { get; set; }//from Patient
        public string PatientName { get; set; }//from Patient
        public string Type { get; set; }//Category from Members
        public int PharmId { get; set; }//foreign key from Pharmacy
        [Required]
        public string MedName { get; set; }
        [Required]
        public string Quantity { get; set; }
        [Required]
        public double Price { get; set; }

        //Foreign key references
        public virtual Pharmastock Pharmastocks{get;set;}
        public virtual Patient Patients{get;set;}

    }
}