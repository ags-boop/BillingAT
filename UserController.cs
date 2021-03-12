using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BillingEnd.Models;
using System.Web;
using System.Data.SqlClient;
using System.Security.Principal;
namespace BillingEnd.Controllers
{
    public class UserController : Controller
    {
        private readonly db _context;

        public UserController(db context)
        {
            _context = context;
        }



        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(User model)
        {

            if (ModelState.IsValid)
            {
                bool IsValidUser = model.UserName == "Daniel@logistica.com" && model.Password == "danieladmin";
                bool IsValidUser2 = model.UserName == "Vendedores@logistica.com" && model.Password == "vendedoresadmin";


                TempData["isValid"]= IsValidUser;
                if (IsValidUser || IsValidUser2)
                {
                    return RedirectToAction("Index", "Producto");
                }
            }
            ModelState.AddModelError("", "invalid Username or Password");
            return View("Index");


        }
    }
}