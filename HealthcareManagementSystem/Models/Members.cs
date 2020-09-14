using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace HealthcareManagementSystem.Models
{
    public class Members
    {
        [Key]
        public int opno { get; set; }
        [Required(ErrorMessage ="Enter the name.")]
        public string Name { get; set; }
        [Required]
        public string Department { get; set; }
        public int Age { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Address { get; set; }
        public string Mother { get; set; }
        public string Father { get; set; }
        public string Spouse { get; set; }
        public string Child1 { get; set; }
        public string Child2 { get; set; }
        [Display(Name="Category")]
        public string Type { get; set; }
    }
}