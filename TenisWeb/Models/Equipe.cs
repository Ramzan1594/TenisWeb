using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenisWeb.Models
{
    public class Equipe
    {
		//Propriete get set pour avoir/attribue les valeurs aux variables
		public int IdEquipe { get; set; }
		public String Nom	{ get; set; }
		public int J1	{ get; set; }
		public int J2	{ get; set; }
		public String Joueur1 { get; set; }
		public String Joueur2 { get; set; }


		//REDEFINITION DE LA METHODE TOSTRING   
		public override String ToString() { return IdEquipe + "." + Nom + " : " + J1 + " et " + J2; }


		//Constructeur
		public Equipe() { }    //par defaut

		public Equipe(Equipe e)
		{
			this.IdEquipe = e.IdEquipe;
			this.Nom = e.Nom;
			this.J1 = e.J1;
			this.J2 = e.J2;
		}

		public Equipe(int id, String nom, int j1, int j2)
		{
			this.IdEquipe = id;
			this.Nom = nom;
			this.J1 = j1;
			this.J2 = j2;
		}

		public Equipe(int id, String nom, String j1, String j2)
		{
			this.IdEquipe = id;
			this.Nom = nom;
			this.Joueur1 = j1;
			this.Joueur2 = j2;
		}
	}
}
