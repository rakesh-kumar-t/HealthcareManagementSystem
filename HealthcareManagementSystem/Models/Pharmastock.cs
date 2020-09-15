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
        public int DrugId { get; set; }//Foreign key from drugstore
        [Required]
        public DateTime Expiry { get; set; }
        [Required]
        public DateTime Dateadded { get; set; }
        [Required]
        public double Price { get; set; }

        //Foreign Key references
        public virtual DrugHouse DrugHouses{get;set;}

        public virtual ICollection<Pharmacy> Pharmacies{get;set;}
    }
}