using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HealthcareManagementSystem.Models
{
    public class DrugHouse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DrugId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime ManufactureDate { get; set; }
        [Required]
        public DateTime ExpiryDate { get; set; }
        [Required]
        public int StockLeft { get; set; }
        [Required]
        public double Price { get; set; }

        public virtual ICollection<Pharmastock> Pharmastocks{get;set;}

    }
}