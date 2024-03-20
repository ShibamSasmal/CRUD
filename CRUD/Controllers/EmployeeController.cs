using CRUD.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRUD.Controllers
{
    public class EmployeeController : Controller
    {
        EmployeeCRUDEntities db = new EmployeeCRUDEntities();
        // GET: Employee
        public ActionResult Index()
        {
            var data = db.Employees.ToList();
            return View(data);
        }

        [HttpPost]
        public ActionResult AddEmployee(Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                int a = db.SaveChanges();
                if (a > 0)
                {
                    TempData["InsertMessage"] = "<script>alert('Data Inserted')</script>";
                    return RedirectToAction("Index");
                }

                else
                {
                    TempData["InsertMessage"] = "<script>alert('Data not Inserted')</script>";
                }
            }
          
            return View();
        }

        public ActionResult EditEmployee(int id)
        {
            var row = db.Employees.Where(model => model.Id == id).FirstOrDefault();
            return PartialView("_EditEmployeeModal", row);
            //return View(row); 
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditEmployee(Employee employee)
        {

            if (ModelState.IsValid)
            {
                db.Entry(employee).State = System.Data.Entity.EntityState.Modified;
                int a = db.SaveChanges();
                if (a > 0)
                {
                    TempData["UpdateMessage"] = "<script>alert('Data Updated successfully')</script>";
                    return RedirectToAction("Index");
                }

                else
                {
                    TempData["UpdateMessage"] = "<script>alert('Data not Updated')</script>";
                }
            }
            return View();
        }

        
        public ActionResult DeleteEmployee(int id)
        {
            var deletedrow = db.Employees.Where(model => model.Id == id).FirstOrDefault();
            return PartialView("_DeleteEmployeeModal", deletedrow);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult DeleteEmployee(Employee e)
        {
            db.Entry(e).State = System.Data.Entity.EntityState.Deleted;
            int a = db.SaveChanges();
            if (a > 0)
            {
                TempData["DeleteMessage"] = "<script>alert('Deleted')</script>";
                return RedirectToAction("Index");
            }

            else
            {
                TempData["DeleteMessage"] = "<script>alert('Not Deletdd')</script>";
            }
            return View();
        }

        [HttpPost]
        public JsonResult DeleteAllEmployee(List<int> e)
        {
            try
            {
                if(e != null)
                {
                    if(e.Count < 1)
                    {
                        return Json(new
                        {
                            msg = String.Format("No data found.")
                        });

                    }
                    using (EmployeeCRUDEntities dbase = new EmployeeCRUDEntities())
                    {
                        var aa = dbase.Employees.Where(x => e.Contains(x.Id)).ToList();
                        dbase.Employees.RemoveRange(aa);
                        int a = dbase.SaveChanges();
                        return Json(new
                        {
                            msg = String.Format("Delete Successfull")
                        });
                    }

                }
                else
                {
                    return Json(new
                    {
                        msg = String.Format("No data found.")
                    });

                }
                
            }
            catch (Exception ex) {
                return Json("Error"+ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

    }
}