using HealthcareManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace HealthcareManagementSystem.Controllers
{
    public class PharmacyController : Controller
    {
        private HealthCareContext db = new HealthCareContext();

        // GET: Pharmacy
        [Authorize]
        public ActionResult Index()
        {
            if (Session["UserId"] != null && Session["Role"].ToString() == "Manager")
                return View(db.Pharmastocks.ToList());
            else
                return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public ActionResult AddStock()
        {
            if (Session["UserId"] != null && Session["Role"].ToString() == "Manager")
            {
                Session["DropDown"] = new SelectList(db.DrugHouses, "DrugId", "Name");
                return View();
            }
            else
                return RedirectToAction("Index", "Home");
        }
        [Authorize]
        [HttpPost]
        public ActionResult AddStock(Pharmastock pharm)
        {
            if (Session["UserId"] != null && Session["Role"].ToString() == "Manager")
            {
                if (ModelState.IsValid)
                {
                    db.Pharmastocks.Add(pharm);
                    db.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Data Format.");
                }
                return View(pharm);
            }
            else
                return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public ActionResult ViewReport()
        {
            if (Session["UserId"] != null && Session["Role"].ToString() == "Manager"||Session["Role"].ToString() == "Admin")
            {
                return View(db.Pharmacies.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
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