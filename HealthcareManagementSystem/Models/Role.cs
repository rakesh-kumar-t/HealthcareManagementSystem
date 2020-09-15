using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthcareManagementSystem.Models
{
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId{get;set;}
        [Required]
        public string RoleName{get;set;}

        //Foreign Key references
        public virtual ICollection<User> Users{get;set;}
    }
}