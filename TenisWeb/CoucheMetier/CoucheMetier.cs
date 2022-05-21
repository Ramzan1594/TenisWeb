using System;
using System.Collections.Generic;
using TenisWeb.Models;
using System.Text.RegularExpressions;

namespace TenisWeb.coucheMetier
{
    public class ClassCoucheMetier
    {
		/**
		 * Methode qui test si les donnees sur un joueur sont  valide et respectes les contrainte metier
		 * @param nom
		 * @param prenom
		 * @param style
		 * @param nomImage
		 * @return   joueur
		 */
		public Joueur TesterContraintesJoueur(String nom, String prenom, String style, String nomImage) 
		{
			Joueur joueur = new Joueur();
		
			if(nom.Trim().CompareTo("") == 0)
				throw new ExceptionMetier("La chaine du nom est vide");

			joueur.Nom = nom;

			if(prenom.Trim().CompareTo("") == 0)
				throw new ExceptionMetier("La chaine du prenom est vide");

			joueur.Prenom	= prenom;
			joueur.StyleJeu = style;
		
			if(nomImage == null || nomImage.Trim().CompareTo("") == 0)
				joueur.Photo= "aucune.jpg";
			else
				joueur.Photo = nomImage ;
		
				
			return joueur;
		}

		/**
		 *  Methode qui test si les donnees sur une equipe sont  valide et respectes les contrainte metier
		 * @param nom
		 * @param joueur1
		 * @param joueur2
		 * @return
		 */
		public Equipe TesterContraintesEquipe(String nom, int joueur1, int joueur2) 
		{	
			Equipe equipe = new Equipe();
		
			if(nom.Trim().CompareTo("") == 0)
				throw new ExceptionMetier("La chaine du nom est vide");

			equipe.Nom = nom;
			if (joueur1.CompareTo(joueur2) == 0)
				throw new ExceptionMetier("Vous ne pouvez choisir deux fois le meme joueur!");
			equipe.J1 = joueur1;
			equipe.J2 = joueur2;
		
			return equipe;
		}

		/**
		 *  Methode qui test si les donnees sur une rencontre sont  valide et respectes les contrainte metier APRES MODIFICATION
		 * @param phase
		 * @param e1
		 * @param e2
		 * @param arbitre
		 * @param table
		 * @param gagnant
		 * @param resultE1
		 * @param resultE2
		 * @return
		 */
		public Rencontre TesterContraintesModifierRencontre(int id, String phase, int e1, int e2, int arbitre, int table, int gagnant, int resultE1, int resultE2)
		{
			Regex pattern = new Regex(@"([1-9])+([0-9])*( )*(-)+( )*([1-9])+([0-9])*");
			Rencontre rencontre = new Rencontre();

			rencontre.IdRencontre = id;

			if (phase.Trim().CompareTo("") == 0)
				throw new ExceptionMetier("La chaine de la phase est vide");

			rencontre.Phase = phase;
			rencontre.E1 = e1;
			rencontre.E2 = e2;
			rencontre.Arbitre = arbitre;
			rencontre.Table = table;

			if (gagnant.CompareTo(e1) != 0 && gagnant.CompareTo(e2) != 0)
				throw new ExceptionMetier("Le gagnant doit etre une des deux equipe de la rencontre");
			rencontre.Gagnant = gagnant;

			//if (!pattern.IsMatch(resultat))
			if (resultE1 == 0 || resultE2 == 0)
				throw new ExceptionMetier("ATTENTION ! Les deux equipes doivent imperativement avoir un resultat");
			//	throw new ExceptionMetier("Le resultat que vous avez entre doit etre sous le format Resultat Equipe1 - Resultat Equipe1 --> exemple: 52 - 78");
			if ( (gagnant == e1 && resultE1 < resultE2 ) || (gagnant == e2 && resultE2 < resultE1))
				throw new ExceptionMetier("ATTENTION ! Le gagant ne concorde pas avec les points donnés aux equipes");

			rencontre.ResultatE1 = resultE1;
			rencontre.ResultatE2 = resultE2;
			
			return rencontre;
		}


		/**
		 * Methode qui test si les donnees sur une rencontre pour une modification sont  valide et respectes les contrainte metier  
		 * @param phase
		 * @param idEquipe1
		 * @param idEquipe1
		 * @param idArbitre
		 * @param idTable
		 * @param resultE1
		 * @param resultE2
		 * @param resultat
		 * return rencontre
		 */
		public Rencontre TesterContraintesRencontre(string phase, int idEquipe1, int idEquipe2, int idArbitre, int idTable, int gagnant, int resultE1, int resultE2, string resultat)
		{
			Rencontre rencontre = new Rencontre();

			if (phase.Trim().CompareTo("") == 0)
				throw new ExceptionMetier("La chaine de la phase est vide");

			rencontre.Phase = phase;
			rencontre.E1 = idEquipe1;
			rencontre.E2 = idEquipe2;
			rencontre.Arbitre = idArbitre;
			rencontre.Table = idTable;
			rencontre.Gagnant = gagnant;
			rencontre.ResultatE1 = resultE1;
			rencontre.ResultatE2 = resultE2;
			rencontre.Resultat = resultat;			

			return rencontre;
		}
	}
}
