using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace TenisWeb.Models
{
    public class Arbitre
    {
		//Propriete get set pour avoir/attribue les valeurs aux variables
		public int IdArbitre { get; set; }
		public String Nom { get; set; }
		public String Prenom { get; set; }
		public int AnneeExperience { get; set; }

		//REDEFINITION DE LA METHODE TOSTRING   
		public override String ToString() { return Nom + " " + Prenom; }


		//Constructeur
		public Arbitre() { }    //par defaut

		public Arbitre(Arbitre a)
		{
			this.IdArbitre = a.IdArbitre;
			this.Nom = a.Nom;
			this.Prenom = a.Prenom;
			this.AnneeExperience = a.AnneeExperience;
		}

		public Arbitre(int id, String nom, String prenom, int anneeExp)
		{
			this.IdArbitre = id;
			this.Nom = nom;
			this.Prenom = prenom;
			this.AnneeExperience = anneeExp;
		}
	}
}
