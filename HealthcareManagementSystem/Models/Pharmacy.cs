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
        public int PharmId { get; set; }//foreign key from Pharmacy
        [Required]
        public int Quantity { get; set; }
        [Required]
        public double Price { get; set; }
        public double TotalAmount { get; set; }
        public DateTime Date { get; set; }

        //Foreign key references
        public virtual Pharmastock Pharmastocks{get;set;}
        public virtual Patient Patients{get;set;}

    }
}