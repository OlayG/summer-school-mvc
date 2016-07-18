using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using summer_school_mvc.Models;

namespace summer_school_mvc.Controllers
{
    public class StudentsController : Controller
    {
        private SummerSchoolMVCEntities db = new SummerSchoolMVCEntities();

        // GET: Students
        public ActionResult Index()
        {
            ViewBag.totalSum = totalSum();
            return View(db.Students.ToList());
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentID,FirstName,LastName,Balance")] Student student)
        {
            student.EnrollmentFee = EnrollmentFeeCalc(student.FirstName, student.LastName);
            student.Balance = student.EnrollmentFee;

            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(student);
        }

        private decimal EnrollmentFeeCalc(string first_name, string last_name)
        {
            decimal EnrollmentCost = 200;

            string full_name = first_name + last_name;

            //if (last_name.ToLower() == "malfoy")
            //{
            //    Console.WriteLine("Enrollment declined. The Malfoy family has been BANNED from our institution.");
            //}
            ////else if (WeasleyOrGranger(last_name))
            ////{
            ////    return EnrollmentCost * 0;
            ////    //if (ProfessorMcgonagallSpecialNames(full_name))
            ////    //{
            ////    //    Console.WriteLine("Thank you for choosing us {0}.\nYour currently balance is an amount of: £{1}", SpecialMessage, StudentsAccountBalance[openSpot]);
            ////    //}
            ////    //else
            ////    //{
            ////    //    Console.WriteLine("Thank you for choosing us {0}.\nYour currently balance is an amount of: £{1}", StudentName, StudentsAccountBalance[openSpot]);
            ////    //}

            ////}
            //else 
            if (full_name.ToLower().Contains("potter"))
            {
                double temp = Convert.ToDouble(EnrollmentCost);
                temp = temp * .5;

                EnrollmentCost = Convert.ToDecimal(temp);

                return EnrollmentCost;
            }
            else if (full_name.ToLower().Contains("longbottom"))
            {

                if (db.Students.Count() < 10)
                {

                    return EnrollmentCost * 0;
                }
                else
                {
                    return EnrollmentCost;
                }


            }
            else if (first_name.ToLower().First() == last_name.ToLower().First())
            {
                double temp = Convert.ToDouble(EnrollmentCost);
                temp = temp * .90;

                EnrollmentCost = Convert.ToDecimal(temp);
                return EnrollmentCost;
            }
            else if (CheckStudentConnectionToQuidditchTeam(last_name) == true)
            {
                double temp = Convert.ToDouble(EnrollmentCost);
                temp = temp * .70;

                EnrollmentCost = Convert.ToDecimal(temp);
                return EnrollmentCost;
            }
            else
            {
                return EnrollmentCost;
            }

        }

        //private bool WeasleyOrGranger(Student student)
        //{
        //    string Lname = student.LastName;

        //    // Used to see if students enrolled qualify for the WeasleyGranger Scholarship
        //    if (Lname.ToLower() == "weasley" || Lname.ToLower() == "granger")
        //    {
        //        for (int i = 0; i < db.Students.Count(); i++)
        //        {
        //            // Checks to see if student already recieved this schorlarship before applying it
        //            if (!WeasleyGrangerDiscount[i])
        //                StudentsAccountBalance[i] = StudentsAccountBalance[i] * .95;
        //        }
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }

        //}

        private static bool ProfessorMcgonagallSpecialNames(string StudentName)
        {
            // Checks to see if any part of student's name contains any of names Professor McGonagall Privacy List
            if (StudentName.ToLower().Contains("tom") || StudentName.ToLower().Contains("riddle") || StudentName.ToLower().Contains("voldemort"))
            {
                return true;
            }
            return false;
        }

        private static bool CheckStudentConnectionToQuidditchTeam(string Lname)
        {
            string[] EnglishNationalQuidditchTeam = { "Vosper", "Hawksworth", "Flitney", "Withey",
                                                        "Choudry", "Frisby", "Parkin" };
            // Compares last name of user to those listed in the wuidditch team array
            for (int i = 0; i < EnglishNationalQuidditchTeam.Length; i++)
            {
                if (Lname == EnglishNationalQuidditchTeam[i])
                {
                    return true;
                }
            }

            return false;

        }

        private decimal totalSum()
        {
            decimal totalSum = 0;

            foreach(Student student in db.Students)
            {
                totalSum = totalSum + student.EnrollmentFee;
            }
            return totalSum;
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentID,FirstName,LastName,EnrollmentFee,Balance")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
