using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HealthcareManagementSystem.Models
{
    public class Patient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Key]
        public int Opno { get; set; }

        [Required(ErrorMessage = "Enter the  patient name.")]
        public string PatientName { get; set; }

        [Required(ErrorMessage = "Enter the patient type")]
        public string Type { get; set; }

        [Required]
        public string DoctorName { get; set; }

        [Required]
        public string Disease { get; set; }

        public double BillAmount { get; set; }

        [Required]
        public DateTime Date { get; set; }

    }
}