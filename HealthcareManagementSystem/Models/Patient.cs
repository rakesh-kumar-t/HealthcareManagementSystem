using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthcareManagementSystem.Models
{
    public class Patient
    {
        public int id { get; set; }
        public int opno { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string doctorname { get; set; }
        public string disease { get; set; }
        public float billamount { get; set; }
        public DateTime date { get; set; }

    }
}