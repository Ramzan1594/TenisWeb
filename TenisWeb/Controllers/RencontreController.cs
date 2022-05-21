using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TenisWeb.CoucheAccesDB;
using TenisWeb.coucheMetier;
using TenisWeb.Models;

namespace TenisWeb.Controllers
{
    public class RencontreController : Controller
    {
        private FabriqueDAO fabriqueDAO = new FabriqueDAO();
        private readonly IHostingEnvironment HEnv;

        /* constructeur
        Param hEnv : ce paramètre nous servira ultérieurement à obtenir le chemin d'accès au dossier de base du projet */
        public RencontreController(IHostingEnvironment hEnv)
        {
            HEnv = hEnv;
        }

        [HttpGet]
        public IActionResult ListerTous()
        {
            List<Rencontre> ListeRencontres = new List<Rencontre>();
            try
            {
                List<Rencontre> Rencontres = fabriqueDAO.GetRencontreDAO().ListerTous();

                foreach (Rencontre rencontre in Rencontres)
                {
                    Equipe e1 = fabriqueDAO.GetEquipeDAO().Charger(rencontre.E1);
                    Equipe e2 = fabriqueDAO.GetEquipeDAO().Charger(rencontre.E2);
                    Equipe g  = fabriqueDAO.GetEquipeDAO().Charger(rencontre.Gagnant);
                    Arbitre a = fabriqueDAO.GetArbitreDAO().Charger(rencontre.Arbitre);

                    Rencontre r = new Rencontre();

                    r.IdRencontre   = rencontre.IdRencontre;
                    r.Phase         = rencontre.Phase;
                    r.Equipe1       = e1.Nom;
                    r.Equipe2       = e2.Nom;
                    if(g != null)
                        r.GagnantString = g.Nom;
                    r.ArbitreString = a.Nom + "  " + a.Prenom;
                    r.Table         = rencontre.Table;
                    r.Resultat      = rencontre.Resultat;

                    ListeRencontres.Add(r);
                }

                ViewBag.ListeRencontre = ListeRencontres;

                if (ViewBag.ListeRencontre.Count == 0)
                    throw new ExceptionAccesDB("Le tournoi n'a pas encore commencer.");
                return View();
            }
            catch (ExceptionAccesDB e)
            {
                ViewBag.Message = e.Message;
                return View("Erreur");
            }
        }

        /* Méthode qui sélectionne la vue permettant de modifier une rencontre existant
        Param numRencontre : le numéro de la rencontre à modifier */
        [HttpGet]
        public IActionResult Modifier([FromQuery]int numRencontre)
        {
            try
            {
                ViewBag.Rencontre = fabriqueDAO.GetRencontreDAO().Charger(numRencontre);
                return View();
            }
            catch (ExceptionAccesDB e)
            {
                ViewBag.Message = e.Message;
                return View("Erreur");
            }
        }


        /* Méthode exécutée quand l'utilisateur valide la modification d'une rencontre
        Elle modifie dans la BD la rencontre
        Param rencontre : l'objet contenant les informations sur la rencontre */
        [HttpPost]
        public IActionResult ModifierSuite(Rencontre rencontre)
        {
            try
            {
                // vérifier si les infos encodées sont valides
                //new CoucheMetier().TesterContraintesModifierRencontre(rencontre.IdRencontre, rencontre.Phase, rencontre.E1, rencontre.E2, rencontre.Arbitre, rencontre.Table, rencontre.Gagnant, rencontre.Resultat);
                new ClassCoucheMetier().TesterContraintesModifierRencontre(rencontre.IdRencontre, rencontre.Phase, rencontre.E1, rencontre.E2, rencontre.Arbitre, rencontre.Table, rencontre.Gagnant,
                                                                        rencontre.ResultatE1, rencontre.ResultatE2);
                Rencontre laRencontre = new Rencontre(rencontre.IdRencontre, rencontre.Phase, rencontre.E1, rencontre.E2, rencontre.Arbitre, rencontre.Table, rencontre.Gagnant, rencontre.ResultatE1,
                                                        rencontre.ResultatE2, rencontre.ResultatE1+ " - " +rencontre.ResultatE2);
                // débuter une transaction
                fabriqueDAO.DebuterTransaction();
                // modifier l'élève dans la bd
                fabriqueDAO.GetRencontreDAO().Modifier(laRencontre);
                // on sait que l'upload du fichier a réussi. On peut valider l'ajout dans la BD
                fabriqueDAO.ValiderTransaction();
                return RedirectToAction("ListerTous");
            }
            catch (ExceptionMetier e)
            {
                ViewBag.Message = e.Message;
            }
            catch (Exception e)
            {
                // on sait que l'upload du fichier a échoué. On doit annuler l'ajout dans la BD
                fabriqueDAO.AnnulerTransaction();
                ViewBag.Message = e.Message;
            }
            return View("Erreur");
        }


    }
}