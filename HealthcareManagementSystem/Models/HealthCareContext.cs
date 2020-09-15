using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace HealthcareManagementSystem.Models
{
    public class HealthCareContext:DbContext
    {
        public HealthCareContext():base("name=Dbconfig")
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<DrugHouse> DrugHouses { get; set; }
        public DbSet<Pharmacy> Pharmacies { get; set; }
        public DbSet<Pharmastock> Pharmastocks { get; set; }
    }
}