using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TenisWeb.Models;

namespace TenisWeb.Controllers
{
    public class HomeController : Controller
    {
        //Methode qui selectionne la vue de depart du site
        // avoir une vue dans Views/Home/memeNom.cshtml   Index.cshtml 
        //ce qui fait le lien auto
        public IActionResult Index() 
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
