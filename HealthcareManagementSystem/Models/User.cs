using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthcareManagementSystem.Models
{
    public class User
    {
        [Key]
        public string UserId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [NotMapped]
        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string Confirmpassword { get; set; }
        [Required]
        public int RoleId { get; set; }

        //Foreign key references
        public virtual Role Roles{get;set;}
    } 
}