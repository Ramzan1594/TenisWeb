using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TenisWeb.CoucheAccesDB;

namespace TenisWeb.Controllers
{
    public class TableController : Controller
    {
        private FabriqueDAO fabriqueDAO = new FabriqueDAO();

        //Methode qui selectionne la vue qui liste tous les tables
        [HttpGet]
        public IActionResult ListerTous()
        {
            try
            {
                ViewBag.ListeTable = fabriqueDAO.GetTableDAO().ListerTous();
                return View();
            }
            catch (ExceptionAccesDB e)
            {
                ViewBag.Message = e.Message;
                return View("Erreur");
            }
        }
    }
}