using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectManagementTool.Controllers
{
    [Authenticated]
    public class PGMToolController : Controller
    {
        // GET: PGMTool

        public ActionResult Manage()
        {

            return View();
        }

        //Project Views
        public ActionResult Projects(string p="0")
        {
            if (p.Equals("0"))
            {
                return RedirectToAction("OverView");
            }
            return PartialView("_PGMProjectList");
        }

        //Overview
        public ActionResult OverView(string partial)
        {
         
            return View();
        }
    }
}