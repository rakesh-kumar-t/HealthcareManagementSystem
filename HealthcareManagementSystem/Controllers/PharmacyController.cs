using HealthcareManagementSystem.Models;
using Microsoft.Ajax.Utilities;
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
                return View(db.DrugHouses.Where(d=>d.StockLeft>0).OrderBy(exp=>exp.ExpiryDate).ToList());
            }
            else
                return RedirectToAction("Index", "Home");
        }
        [Authorize]
        [HttpPost]
        public ActionResult AddStock(int? DrugId,int? Itemno)
        {
            if (Session["UserId"] != null && Session["Role"].ToString() == "Manager")
            {
                if (DrugId==null || Itemno==null)
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
                var Drug = db.DrugHouses.Find(DrugId);
                if (Drug == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    Pharmastock pharm = new Pharmastock();
                    pharm.DrugId=Drug.DrugId;
                    pharm.Stockleft = (int)Itemno;
                    pharm.Expiry = Drug.ExpiryDate;
                    pharm.Dateadded = DateTime.Now;
                    db.Pharmastocks.Add(pharm);
                    db.SaveChanges();
                }
                return View();
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