using HealthcareManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.Entity;


namespace HealthcareManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private HealthCareContext db=new HealthCareContext();
        public ActionResult Index()
        {
            string userid = "admin@demo.com";
            string password = "Admin@123";
            var o = db.Users.Where(u => u.UserId.Equals(userid)).FirstOrDefault();
            if (o == null)
            {
                //Add admin role in Role Table
                var RoleTable = db.Roles.ToList();
                if (RoleTable.ToList().Count == 0)
                {
                    Role usr = new Role();
                    usr.RoleName = "Admin";
                    db.Roles.Add(usr);
                    db.SaveChanges();
                    usr.RoleName = "Manager";
                    db.Roles.Add(usr);
                    db.SaveChanges();
                    usr.RoleName = "Reception";
                    db.Roles.Add(usr);
                    db.SaveChanges();
                    usr.RoleName = "Pharmacy";
                    db.Roles.Add(usr);
                    db.SaveChanges();
                    usr.RoleName = "Nurse";
                    db.Roles.Add(usr);
                    db.SaveChanges();
                }
                User admin = new User();
                admin.Name = "Adminname"; 
                admin.UserId = userid;
                admin.Password = Encrypt(password);
                admin.Confirmpassword = Encrypt(password);
                admin.RoleId = 1;
                db.Users.Add(admin);
                db.SaveChanges();
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(User Recept)
        {
            Recept.Password = Encrypt(Recept.Password);
            var reception = db.Users.Where(r => r.UserId.Equals(Recept.UserId) && r.Password.Equals(Recept.Password)).FirstOrDefault();
            if(reception!=null)
            {
                FormsAuthentication.SetAuthCookie(Recept.UserId,false);
                Session["UserId"] = reception.UserId.ToString();
                Session["Name"] = reception.Name.ToString();
                Session["Role"] = reception.Roles.RoleName.ToString();
                string role = Session["Role"].ToString();
                if (role == "Admin")
                    return RedirectToAction("Index", "Admin");
                else if (role == "Manager")
                    return RedirectToAction("Index", "Pharmacy");
                else if (role == "Nurse")
                    return RedirectToAction("ViewPatient", "Home");
                else if (role == "Pharmacy")
                    return RedirectToAction("ViewPatient", "Home");
                else if (role == "Reception")
                    return RedirectToAction("ViewPatient", "Home");
                else
                    return RedirectToAction("Index", "Home");
            }
            else
            { 
                ModelState.AddModelError("", "Invalid credentials"); 
            }
            return View(Recept);
        }

        public ActionResult About()
        {
            ViewBag.Message = "The world perceives us as a low-cost Indian healthcare service provider; what we are engaged in is a passionate journey to establish ourselves as the lowest - cost, high - quality healthcare service provider in the world.At Clinic Relief, we are convinced that 'quality' and 'lowest cost' are not mutually exclusive when it comes to healthcare delivery.In fact, we are well on our way to demonstrate that we are not running our institution as just another number-only business, but are attractively placed to create an affordable, globally-benchmarked quality - driven healthcare services model.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        
        //NewPatient 
        [Authorize]
        public ActionResult NewPatient()
        {
            if (Session["UserId"] != null && Session["Role"].ToString() == "Reception" )
            {
                ViewBag.Memberlist = db.Members;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        [Authorize]
        public ActionResult NewPatient(Patient pt)
        {
            if (Session["UserId"] != null && Session["Role"].ToString() == "Reception")
            {
                if (ModelState.IsValid)
                {
                    pt.Date = DateTime.Now;
                    db.Patients.Add(pt);
                    db.SaveChanges();
                    return RedirectToAction("ViewPatient");
                }
                return View(pt);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult ViewPatient()
        {
            if (Session["UserId"] != null && (Session["Role"].ToString() == "Reception" || Session["Role"].ToString() == "Nurse" || Session["Role"].ToString() == "Pharmacy"))
            {
                return View(db.Patients.OrderByDescending(o=>o.Date).ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        //PatientDetails method
        [Authorize]
        public ActionResult PatientDetails(int? id)
        {
            if (id != null)
            {
              var patient=  db.Patients.Find(id);
                if(patient!=null)
                {
                    return View(patient);
                }
            }
            return View();
        }

        
        [Authorize]
        public ActionResult AddService(int? id)
        {
            ViewBag.PatientList = db.Patients.ToList();
            if (id != null)
            {
                var patient = db.Patients.Find((int)id);
                if (patient != null)
                {
                    Service service = new Service();
                    service.PId = patient.PId;
                    return View(service);
                }
            }
            return View();
        }
        [HttpPost]
        [Authorize]
        public ActionResult AddService(Service serve)
        {
            if (Session["Role"].ToString() == "Nurse" || Session["Role"].ToString() == "Reception"||Session["Role"].ToString()=="Pharmacy")
            {
                ViewBag.PatientList = db.Patients.ToList();
                if (ModelState.IsValid)
                {
                    serve.Date = DateTime.Now;
                    serve.TotalAmount = serve.Quantity * serve.Price;
                    db.Services.Add(serve);
                    db.SaveChanges();
                    var patient = db.Patients.Find(serve.PId);
                    patient.BillAmount += serve.Price;
                    db.Entry(patient).State =EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.Status = "success";
                    ViewBag.Message = "New service added to patient successfully";
                }
                return View(serve);
            }
            else
                return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public ActionResult AddMedicine(int? id)
        {
            if (Session["UserId"]!=null&&(Session["Role"].ToString() == "Nurse" || Session["Role"].ToString() == "Pharmacy" || Session["Role"].ToString() == "Reception"))
            {
                ViewBag.PatientList = db.Patients.ToList();
                ViewBag.DrugList = db.Pharmastocks.ToList();
                if (id != null)
                {
                    var patient = db.Patients.Find((int)id);
                    if (patient != null)
                    {
                        Pharmacy newmedicine = new Pharmacy();
                        newmedicine.PId = patient.PId;
                        return View(newmedicine);
                    }

                }
                return View();
            }
            else
                return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [Authorize]
        public ActionResult AddMedicine(Pharmacy pharm)
        {
            if (Session["Role"].ToString() == "Nurse" || Session["Role"].ToString() == "Pharmacy" || Session["Role"].ToString() == "Reception")
            {
                if (ModelState.IsValid)
                {
                    var pharmastock = db.Pharmastocks.Find(pharm.PharmId);
                    pharm.Date = DateTime.Now;
                    pharm.Price = pharmastock.Price;
                    db.Pharmacies.Add(pharm);
                    db.SaveChanges();
                    var patient = db.Patients.Find(pharm.PId);
                    patient.BillAmount += pharm.Price;
                    db.Entry(patient).State=EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.Status = "success";
                    ViewBag.Message = "New medicine added to patient successfully";
                }
                ViewBag.PatientList = db.Patients.ToList();
                ViewBag.DrugList = db.Pharmastocks.ToList();
                return View(pharm);
            }
            else
                return RedirectToAction("Index", "Home");
        }





        //COMMON METHODS FOR ALL USERS DEFINED HERE
        //Change name for all users
        [Authorize]
        public ActionResult Settings()
        {
            if (Session["UserId"] != null)
                return View();
            else
                return RedirectToAction("Index", "Home");
        }
        [Authorize]
        [HttpPost]
        public ActionResult Settings(User usr)
        {
            string username = User.Identity.Name;
            User user = db.Users.FirstOrDefault(u => u.UserId.Equals(username));
            user.Name = usr.Name;
            user.Password = user.Password;
            user.Confirmpassword = user.Password;
            Session["Name"] = user.Name.ToString();
            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return View(usr);
        }
        //Change Password for user
        [Authorize]
        public ActionResult ChangePassword()
        {
            if (Session["UserId"] != null)
                return View();
            else
                return RedirectToAction("Index", "Home");
        }
        //Post method for changepassword
        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(User usr)
        {
            if (Session["UserId"] != null)
            {
                usr.Password = Encrypt(usr.Password);
                usr.Confirmpassword = Encrypt(usr.Confirmpassword);
                string username = User.Identity.Name;
                User user = db.Users.FirstOrDefault(u => u.UserId.Equals(username));
                if (user != null)
                {
                    user.Password = usr.Password;
                    user.Confirmpassword = usr.Confirmpassword;
                    db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.Status = "success";
                    ViewBag.Message = "Password Changed Successfully";
                }
                else
                {
                    ViewBag.Status = "danger";
                    ViewBag.Message = "Error changing password";
                }
                return RedirectToAction("ChangePassword");
            }
            else
                return RedirectToAction("Index", "Home");
        }
        
        //User Logout action
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index");
        }

        //Encrypt password method
        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (System.IO.MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        //Dispose the database
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}