using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenisWeb.Models
{
    public class Joueur
    {
		//Propriete get set pour avoir/attribue les valeurs aux variables
		public int IdJoueur		{ get; set; }
		public String Nom		{ get; set; }
		public String Prenom	{ get; set; }
		public String StyleJeu	{ get; set; }
		public String Photo		{ get; set; }


		//REDEFINITION DE LA METHODE TOSTRING   
		public override String ToString() { return Nom + " " + Prenom; }



		//Constructeur
		public Joueur() { }    //par defaut

		public Joueur(Joueur j)
		{
			this.IdJoueur = j.IdJoueur;
			this.Nom = j.Nom;
			this.Prenom = j.Prenom;
			this.StyleJeu = j.StyleJeu;
			this.Photo = j.Photo;
		}

		public Joueur(int id, String nom, String prenom, String style, String photo)
		{
			this.IdJoueur = id;
			this.Nom = nom;
			this.Prenom = prenom;
			this.StyleJeu = style;
			this.Photo = photo;
		}
	}
}
