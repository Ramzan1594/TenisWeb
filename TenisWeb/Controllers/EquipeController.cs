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
    public class EquipeController : Controller
    {
        private FabriqueDAO fabriqueDAO = new FabriqueDAO();
        private readonly IHostingEnvironment HEnv;

        /* constructeur
        Param hEnv : ce paramètre nous servira ultérieurement à obtenir le chemin d'accès au dossier de base du projet */
        public EquipeController(IHostingEnvironment hEnv)
        {
            HEnv = hEnv;
        }


        [HttpGet]
        public IActionResult ListerTous()
        {
            List<Equipe> ListeEquipe = new List<Equipe>();
            try
            {
                List<Equipe> Equipes = fabriqueDAO.GetEquipeDAO().ListerTous();

                foreach(Equipe equipe in Equipes)
                {
                    Joueur joueur1 = fabriqueDAO.GetJoueurDAO().Charger(equipe.J1);
                    Joueur joueur2 = fabriqueDAO.GetJoueurDAO().Charger(equipe.J2);

                    Equipe e = new Equipe();

                    e.IdEquipe  = equipe.IdEquipe;
                    e.Nom       = equipe.Nom;
                    e.Joueur1   = joueur1.Nom + "  " + joueur1.Prenom;
                    e.Joueur2   = joueur2.Nom + "  " + joueur2.Prenom;

                    ListeEquipe.Add(e);
                }

                ViewBag.ListeEquipe = ListeEquipe;

                if (ViewBag.ListeEquipe.Count == 0)
                    throw new ExceptionAccesDB("Aucune equipe n'est inscrite.");

                return View();
            }
            catch (ExceptionAccesDB e)
            {
                ViewBag.Message = e.Message;
                return View("Erreur");
            }
        }



        //Methode qui selectionne la vue permettant de lister les rencontres d'une equipe
        [HttpGet]
        public IActionResult ListerRencontreSelonEquipe(int idEquipe)
        {
            List<Rencontre> ListeRencontres = new List<Rencontre>();
            try
            {
                List<Rencontre> Rencontres = fabriqueDAO.GetRencontreDAO().ChargerTous(idEquipe);

                foreach (Rencontre rencontre in Rencontres)
                {
                    Equipe e1 = fabriqueDAO.GetEquipeDAO().Charger(rencontre.E1);
                    Equipe e2 = fabriqueDAO.GetEquipeDAO().Charger(rencontre.E2);
                    Equipe g = fabriqueDAO.GetEquipeDAO().Charger(rencontre.Gagnant);
                    Arbitre a = fabriqueDAO.GetArbitreDAO().Charger(rencontre.Arbitre);

                    Rencontre r = new Rencontre();

                    r.IdRencontre = rencontre.IdRencontre;
                    r.Phase = rencontre.Phase;
                    r.Equipe1 = e1.Nom;
                    r.Equipe2 = e2.Nom;
                    if (g != null)
                        r.GagnantString = g.Nom;
                    r.ArbitreString = a.Nom + "  " + a.Prenom;
                    r.Table = rencontre.Table;
                    r.Resultat = rencontre.Resultat;

                    ListeRencontres.Add(r);
                }

                ViewBag.ListeRencontre = ListeRencontres;
                return View();
            }
            catch (ExceptionAccesDB e)
            {
                ViewBag.Message = e.Message;
                return View("Erreur");
            }
        }




        // Méthode qui sélectionne la vue permettant d'encoder une nouvelle equipe
        [HttpGet]
        public IActionResult Ajouter()
        {
            try
            {
                ViewBag.ListeJoueur = fabriqueDAO.GetJoueurDAO().ListerTous();
                return View();
            }
            catch (ExceptionAccesDB e)
            {
                ViewBag.Message = e.Message;
                return View("Erreur");
            }
        }


        /* Méthode exécutée quand l'utilisateur valide l'ajout d'une equipe. Elle enregistre
        dans la BD la nouvelle equipe et 
        Param equipe : l'objet contenant les informations sur l'equipe */
        public IActionResult AjouterSuite(Equipe equipe)
        {
            //on listes les equipes pour voir s'il y en a deja 8
            List<Equipe> lesEquipes = fabriqueDAO.GetEquipeDAO().ListerTous();            

            //liste d'equipe contenant un des deux joueur de l'equipe a ajouter
            int listeEquipe = fabriqueDAO.GetEquipeDAO().ListerEquipeSelonJoueur(equipe.J1, equipe.J2);
                        
            try
            {
                if (lesEquipes.Count >= 8)
                    throw new ExceptionMetier("ATTENTION ! On a deja 8 equipes inscrites");
                if (listeEquipe > 0)
                    throw new ExceptionMetier("Un des deux joueur entree est deja inscris dans une autre equipe !");
            }
            catch (ExceptionMetier e)
            {
                ViewBag.Message = e.Message;
                return View("Erreur");
            }
                            
            try
            {                
                // vérifier si les infos encodées sont valides
                new ClassCoucheMetier().TesterContraintesEquipe(equipe.Nom, equipe.J1, equipe.J2);
                // débuter une transaction
                fabriqueDAO.DebuterTransaction();
                // ajouter le joueur dans la bd
                fabriqueDAO.GetEquipeDAO().Ajouter(equipe);
                
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
                // on sait que l'upload du fichier a échoué. On peut annuler l'ajout dans la BD
                fabriqueDAO.AnnulerTransaction();
                ViewBag.Message = e.Message;
            }
            return View("Erreur");
        }


        /* Méthode qui sélectionne la vue qui propose de supprimer une equipe 
        Param numEquipe : le numéro de l'equipe à supprimer */
        [HttpGet]
        public IActionResult Supprimer([FromQuery]int idEquipe)
        {
            try
            {
                List<Rencontre> rencontre = fabriqueDAO.GetRencontreDAO().ListerTous();
                if (rencontre.Count > 0)
                    throw new ExceptionMetier("Le tournoi a deja commence , aucune suppression n'est permise !");
            }
            catch (ExceptionMetier e)
            {
                ViewBag.Message = e.Message;
                return View("Erreur");
            }

            

            try
            {
                ViewBag.Equipe = fabriqueDAO.GetEquipeDAO().Charger(idEquipe);
                return View();
            }
            catch (ExceptionAccesDB e)
            {
                ViewBag.Message = e.Message;
                return View("Erreur");
            }
        }
        /* Méthode exécutée quand l'utilisateur valide la suppression d'une equipe.
        Elle supprime dans la BD une equipe
        Param numEquipe : le numéro de l'equipe à supprimer */
        [HttpPost]
        public IActionResult SupprimerSuite(int numEquipe)
        {
            try
            {
                fabriqueDAO.DebuterTransaction();
                fabriqueDAO.GetEquipeDAO().SupprimerEquipe(numEquipe);
                fabriqueDAO.ValiderTransaction();
                return RedirectToAction("ListerTous");
            }
            catch (ExceptionAccesDB e)
            {
                fabriqueDAO.AnnulerTransaction();
                ViewBag.Message = e.Message;
                return View("Erreur");
            }
        }
    }
}