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
    public class JoueurController : Controller
    {
        private FabriqueDAO fabriqueDAO = new FabriqueDAO();
        private readonly IHostingEnvironment HEnv;

        /* constructeur
        Param hEnv : ce paramètre nous servira ultérieurement à obtenir le chemin d'accès au dossier de base du projet */
        public JoueurController(IHostingEnvironment hEnv)
        {
            HEnv = hEnv;
        }

        [HttpGet]
        public IActionResult ListerTous()
        {
            try
            {
                ViewBag.ListeJoueur = fabriqueDAO.GetJoueurDAO().ListerTous();

                if (ViewBag.ListeJoueur.Count == 0)
                    throw new ExceptionAccesDB("Aucun joueur n'est inscrit.");

                return View();
            }
            catch (ExceptionAccesDB e)
            {
                ViewBag.Message = e.Message;
                return View("Erreur");
            }
        }

        // Méthode qui sélectionne la vue permettant d'encoder un nouveau joueur
        [HttpGet]
        public IActionResult Ajouter()
        {
            return View();
        }


        /* Méthode exécutée quand l'utilisateur valide l'ajout d'un joueur. Elle enregistre
        dans la BD le nouveau joueur et elle upload le fichier d'image dans le dossier
        du projet \wwwroot\images\joueur
        Param joueur : l'objet contenant les informations sur le joueur
        Param fichierImage : l'objet contenant les informations sur l'image */
        public IActionResult AjouterSuite(Joueur joueur, IFormFile fichierImage)
        {
            try
            {
                if (fichierImage != null)
                    joueur.Photo = Path.GetFileName(fichierImage.FileName);
                else
                    joueur.Photo = "aucune.jpg";
                // vérifier si les infos encodées sont valides
                new ClassCoucheMetier().TesterContraintesJoueur(joueur.Nom, joueur.Prenom, joueur.StyleJeu, joueur.Photo) ;
                // débuter une transaction
                fabriqueDAO.DebuterTransaction();
                // ajouter le joueur dans la bd
                fabriqueDAO.GetJoueurDAO().Ajouter(joueur);
                // upload le fichier
                if (fichierImage != null)
                {
                    FileStream fichier = new FileStream(Path.Combine(HEnv.WebRootPath + "\\images\\joueur\\", Path.GetFileName(fichierImage.FileName)), FileMode.Create);
                    fichierImage.CopyTo(fichier);
                    fichier.Close();
                }
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


        /* Méthode qui sélectionne la vue qui propose de supprimer un joueur
        Param numJoueur : le numéro du joueur à supprimer */
        [HttpGet]
        public IActionResult Supprimer([FromQuery]int idJoueur)
        {
            try
            {
                List<Rencontre>rencontre = fabriqueDAO.GetRencontreDAO().ListerTous();
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
                ViewBag.Joueur = fabriqueDAO.GetJoueurDAO().Charger(idJoueur);
                return View();
            }
            catch (ExceptionAccesDB e)
            {
                ViewBag.Message = e.Message;
                return View("Erreur");
            }
        }
        /* Méthode exécutée quand l'utilisateur valide la suppression d'un joueur.
        Elle supprime dans la BD un joueur et l'equipe
        Param numEleve : le numéro du joueur à supprimer */
        [HttpPost]
        public IActionResult SupprimerSuite(int idJoueur)
        {       
            try
            {
                fabriqueDAO.DebuterTransaction();
                fabriqueDAO.GetEquipeDAO().Supprimer(idJoueur);
                fabriqueDAO.GetJoueurDAO().Supprimer(idJoueur);
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