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
    public class TournoiController : Controller
    {
        private FabriqueDAO fabriqueDAO = new FabriqueDAO();
        private readonly IHostingEnvironment HEnv;

        /* constructeur
        Param hEnv : ce paramètre nous servira ultérieurement à obtenir le chemin d'accès au dossier de base du projet */
        public TournoiController(IHostingEnvironment hEnv)
        {
            HEnv = hEnv;
        }

        [HttpGet]
        public IActionResult GenererTournoi()
        {
            try
            {
                ViewBag.ListeEquipe = fabriqueDAO.GetEquipeDAO().ListerTous();
                ViewBag.ListeRencontre = fabriqueDAO.GetRencontreDAO().ListerTous();
                return View();
            }
            catch (ExceptionAccesDB e)
            {
                ViewBag.Message = e.Message;
                return View("Erreur");
            }
        }




        /*Methode qui est execute quand on appuie sur le boutton Generer quart de final et qui
	     prend la liste des equipe dans la DB , en fait des rencontre aleatoire et les met en DB et les affiche */
        [HttpGet]
        public IActionResult GenererTournoiSuite(List<Equipe> listEquipe)
        {
            List<Equipe> equipes    = listEquipe;
            List<Arbitre> arbitres  = null;
            List<Table> tables      = null;
            List<Rencontre> rencontres = null;

            //Recuperer toutes les listes dans DB
            try
            {
                equipes = fabriqueDAO.GetEquipeDAO().ListerTous();
                arbitres = fabriqueDAO.GetArbitreDAO().ListerTous();
                tables = fabriqueDAO.GetTableDAO().ListerTous();
                rencontres = fabriqueDAO.GetRencontreDAO().ListerTous();

                //Verifie qu'il y a au moin 8 equipes avant de commencer le tournoi
                if (equipes.Count < 8)
                    throw new ExceptionMetier("ATTENTION ! Il faut au minimum 8 equipes pour commencer un tournoi !");

                //Generer les rencontre ALEATOIREMENT et les enregistrer en DB SI ce n'est pas deja fait
                if(rencontres.Count < 4)
                    GenereRencontreAleatoire(equipes, arbitres, tables);

                return Redirect("/Rencontre/ListerTous");
            }
            catch (ExceptionMetier e)
            {
                ViewBag.Message = e.Message;
                return View("Erreur");
            }
        }

            /**
	     * Methode qui genere les rencontre en choisissant des equipes aleatoirement
	     * @param equipes  liste des equipes
	     * @param tables   liste des arbitres
	     * @param arbitres liste des tables
	     */
        private void GenereRencontreAleatoire(List<Equipe> equipes, List<Arbitre> arbitres, List<Table> tables)
        {
            for(int nbRencontre = 0; nbRencontre < 4; nbRencontre++)
            {
                var Random = new Random();
                Rencontre rencontre = new Rencontre();
                Equipe equipeA = new Equipe();
                Equipe equipeB = new Equipe();
                Arbitre arbitre = new Arbitre();
                Table table = new Table();
                int gagnant = 0, resultatEA = 0, resultatEB = 0;

                //Creation des valeurs selectionner aleatoirement pour cree une rencontre
                equipeA = equipes[Random.Next(equipes.Count)]; //la on aura une EQUIPE ALEATOIREMENT 
                equipes.Remove(equipeA);
                equipeB = equipes[Random.Next(equipes.Count)];
                arbitre = arbitres[Random.Next(arbitres.Count)];
                table = tables[Random.Next(tables.Count)];

                //enlever les equipes, arbitres et table deja utilise des list
                equipes.Remove(equipeB);
                arbitres.Remove(arbitre);
                tables.Remove(table);

                /*resultatEA = Random.Next(0, 100) + 1;
                resultatEB = Random.Next(0, 100) + 1;

                if (resultatEA < resultatEB)
                    gagnant = equipeB.IdEquipe ;
                else
                    gagnant = equipeA.IdEquipe;*/

                //tester les contrainte metiers
                try
                {
                    //rencontre = new CoucheMetier().TesterContraintesRencontre("Quart de final", equipeA.IdEquipe, equipeB.IdEquipe, arbitre.IdArbitre, table.IdTable,
                    //                                                                  gagnant, resultatEA + " - " + resultatEB);
                    rencontre = new ClassCoucheMetier().TesterContraintesRencontre("Quart de final", equipeA.IdEquipe, equipeB.IdEquipe, arbitre.IdArbitre, table.IdTable,
                                                                                   gagnant, resultatEA, resultatEB, resultatEA + " - " + resultatEB);
                    fabriqueDAO.DebuterTransaction();
                    // ajouter la rencontre dans la bd
                    fabriqueDAO.GetRencontreDAO().Ajouter(rencontre);
                    // on sait que l'upload du fichier a réussi. On peut valider l'ajout dans la BD
                    fabriqueDAO.ValiderTransaction();
                }
                catch (ExceptionMetier e)
                {
                    ViewBag.Message = e.Message;
                }
            }
        }


        /*Methode qui est execute quand on appuie sur le boutton Generer quart de final et qui
	     prend la liste des equipe dans la DB , en fait des rencontre aleatoire et les met en DB et les affiche */
        [HttpGet]
        public IActionResult GenererDemiFinalFinal(List<Rencontre> listRencontre)
        {
            List<Arbitre> arbitres  = null;
            List<Table> tables      = null;
            List<Rencontre> rencontres = listRencontre;

            //Recuperer toutes les listes dans DB
            try
            {
                arbitres = fabriqueDAO.GetArbitreDAO().ListerTous();
                tables = fabriqueDAO.GetTableDAO().ListerTous();
                rencontres = fabriqueDAO.GetRencontreDAO().ListerTous();

                foreach(Rencontre rencontre in rencontres)
                {
                    if(rencontre.Gagnant == 0)
                        throw new ExceptionMetier("ATTENTION ! Les rencontres des matches precedent doivent TOUTES avoir un gagnant!" 
                        + "   --> Allez dans Lister rencontres / Modifier");
                }

                //Generer les rencontre ALEATOIREMENT et les enregistrer en DB SI ce n'est pas deja fait
                if (rencontres.Count >= 4)
                    GenererRencontreApresQF(rencontres, arbitres, tables);
                else
                    throw new ExceptionMetier("ATTENTION ! Le tournoi n'est pas encore a l'etape que vous avez selectionner !");

                return Redirect("/Rencontre/ListerTous");
            }
            catch (ExceptionMetier e)
            {
                ViewBag.Message = e.Message;
                return View("Erreur");
            }
        }


        /**
	     * Methode qui genere les rencontre apres les QF cad  demi final et final
	     * @param rencontres 
	     * @param tables 
	     * @param arbitres 
	     */
        private void GenererRencontreApresQF(List<Rencontre> rencontres, List<Arbitre> arbitres, List<Table> tables)
        {
            if (rencontres.Count >= 4 && rencontres.Count < 6)  // =================================================================    DEMI FINAL
            {
                for (int nbRencontre = 0; nbRencontre < 2; nbRencontre++)
                {
                    var Random = new Random();
                    Rencontre rencontre = new Rencontre();
                    Arbitre arbitre = new Arbitre();
                    Table table = new Table();
                    int equipeA = 0, equipeB = 0, gagnant = 0, resultatEA = 0, resultatEB = 0;


                    //Creation des valeurs pour cree une rencontre
                    if (nbRencontre == 0)
                    {
                        equipeA = rencontres[0].Gagnant;
                        equipeB = rencontres[1].Gagnant;
                    }
                    if (nbRencontre == 1)
                    {
                        equipeA = rencontres[2].Gagnant;
                        equipeB = rencontres[3].Gagnant;
                    }

                    arbitre = arbitres[Random.Next(arbitres.Count)];
                    table = tables[Random.Next(tables.Count)];

                    //enlever les arbitres et tables deja utilise des list				
                    arbitres.Remove(arbitre);
                    tables.Remove(table);


                    /*resultatEA = Random.Next(0, 100) + 1;
                    resultatEB = Random.Next(0, 100) + 1;

                    if (resultatEA < resultatEB)
                        gagnant = equipeB;
                    else
                        gagnant = equipeA;*/

                    //tester les contrainte metiers - Enregistrer la rencontre en DB
                    try
                    {
                        rencontre = new ClassCoucheMetier().TesterContraintesRencontre("Demi final", equipeA, equipeB, arbitre.IdArbitre, table.IdTable, gagnant, resultatEA, resultatEB, resultatEA + " - " + resultatEB);

                        fabriqueDAO.DebuterTransaction();
                        // ajouter la rencontre dans la bd
                        fabriqueDAO.GetRencontreDAO().Ajouter(rencontre);
                        // on sait que l'upload du fichier a réussi. On peut valider l'ajout dans la BD
                        fabriqueDAO.ValiderTransaction();
                    }
                    catch (ExceptionMetier e)
                    {
                        ViewBag.Message = e.Message;
                    }
                }
            }


            if (rencontres.Count >= 6 && rencontres.Count < 7)    // =================================================================    FINAL
            {
                
                    var Random = new Random();
                    Rencontre rencontre = new Rencontre();
                    Arbitre arbitre = new Arbitre();
                    Table table = new Table();
                    int equipeA = 0, equipeB = 0, gagnant = 0, resultatEA = 0, resultatEB = 0;


                    //Creation des valeurs pour cree une rencontre
                    equipeA = rencontres[4].Gagnant;
                    equipeB = rencontres[5].Gagnant;

                    arbitre = arbitres[Random.Next(arbitres.Count)];
                    table = tables[Random.Next(tables.Count)];

                    //enlever les arbitres et tables deja utilise des list				
                    arbitres.Remove(arbitre);
                    tables.Remove(table);


                    /*resultatEA = Random.Next(0, 100) + 1;
                    resultatEB = Random.Next(0, 100) + 1;

                    if (resultatEA < resultatEB)
                        gagnant = equipeB;
                    else
                        gagnant = equipeA;*/

                    //tester les contrainte metiers - Enregistrer la rencontre en DB
                    try
                    {
                        rencontre = new ClassCoucheMetier().TesterContraintesRencontre("Final", equipeA, equipeB, arbitre.IdArbitre, table.IdTable, gagnant, resultatEA, resultatEB, resultatEA + " - " + resultatEB);

                        fabriqueDAO.DebuterTransaction();
                        // ajouter la rencontre dans la bd
                        fabriqueDAO.GetRencontreDAO().Ajouter(rencontre);
                        // on sait que l'upload du fichier a réussi. On peut valider l'ajout dans la BD
                        fabriqueDAO.ValiderTransaction();
                    }
                    catch (ExceptionMetier e)
                    {
                        ViewBag.Message = e.Message;
                    }
                
            }
        }
        /**
         * Methode qui est exectee quand on clique sur Terminer et termine le tournoi en suppriman toutes les rencontres de la DB
         * */
        [HttpGet]
        public IActionResult Terminer()
        {
            try
            {
                fabriqueDAO.DebuterTransaction();
                // Supprimer toutes les rencontre dans la bd
                fabriqueDAO.GetRencontreDAO().SupprimerTous();

                fabriqueDAO.ValiderTransaction();

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