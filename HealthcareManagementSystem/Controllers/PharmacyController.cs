using HealthcareManagementSystem.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.Entity;


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
                if (Session["Status"] != null)
                {
                    ViewBag.Status = Session["Status"].ToString();
                    ViewBag.Message = Session["Message"].ToString();
                    Session["Status"] = null;
                    Session["Message"] = null;
                }
                return View(db.DrugHouses.Where(d=>d.StockLeft>0).OrderBy(exp=>exp.ExpiryDate).ToList());
            }
            else
                return RedirectToAction("Index", "Home");        }
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
                DateTime alloweddate = DateTime.Now.AddMinutes(2);
                if (Drug == null)
                {
                    Session["Status"] = "danger";
                    Session["Message"] = "Drug Not found, Might have removed from stockhouse";
                    return HttpNotFound();
                }
                else
                {
                    var pharmacy = db.Pharmastocks.Where(medicine => medicine.DrugId == DrugId||medicine.DrugHouses.Name==Drug.Name).FirstOrDefault();
                    if (pharmacy != null)
                    {
                        if (pharmacy.Stockleft > 0)
                        {
                            Session["Status"] = "danger";
                            Session["Message"] = "Stock cant be added when old stock remains";
                        }
                        else
                        {
                            pharmacy.Stockleft = pharmacy.Stockleft + (int)Itemno;
                            pharmacy.Price = Drug.Price;
                            pharmacy.DrugId = Drug.DrugId;
                            pharmacy.Expiry = Drug.ExpiryDate;
                            pharmacy.Dateadded = DateTime.Now;
                            db.Entry(pharmacy).State = EntityState.Modified;
                            db.SaveChanges();
                            Drug.StockLeft = Drug.StockLeft - (int)Itemno;
                            db.Entry(Drug).State = EntityState.Modified;
                            db.SaveChanges();
                            Session["Status"] = "success";
                            Session["Message"] = "Stock Updated successfully";
                        }
                    }
                    else
                    {
                        Pharmastock pharm = new Pharmastock();
                        pharm.DrugId=Drug.DrugId;
                        pharm.Price = Drug.Price;
                        pharm.Stockleft = (int)Itemno;
                        pharm.Expiry = Drug.ExpiryDate;
                        pharm.Dateadded = DateTime.Now;
                        db.Pharmastocks.Add(pharm);
                        db.SaveChanges();
                        Drug.StockLeft = Drug.StockLeft - (int)Itemno;
                        db.Entry(Drug).State = EntityState.Modified;
                        db.SaveChanges();
                        Session["Status"] = "success";
                        Session["Message"] = "New drug added successfully";
                    }
                }
                return RedirectToAction("AddStock");
            }
            else
                return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public ActionResult ClearStock(int? id)
        {
            if(id!=null)
            {
                var pharmacy = db.Pharmastocks.Find((int)id);
                pharmacy.Stockleft = 0;
                db.Entry(pharmacy).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Status = "success";
                ViewBag.Message = "Stocks Cleared Successfully";
            }
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult ViewReport()
        {
            if (Session["UserId"] != null && Session["Role"].ToString() == "Manager"||Session["Role"].ToString() == "Admin")
            {
                return View(db.Patients.OrderByDescending(o=>o.Date).ToList());
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