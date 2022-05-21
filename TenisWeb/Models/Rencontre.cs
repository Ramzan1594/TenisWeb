using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenisWeb.Models
{
    public class Rencontre
    {
		//Propriete get set pour avoir/attribue les valeurs aux variables
		public int IdRencontre	{ get; set; }
		public String Phase		{ get; set; }
		public int E1			{ get; set; }
		public int E2			{ get; set; }
		public int Gagnant		{ get; set; }
		public int Arbitre		{ get; set; }
		public int Table		{ get; set; }
		public int ResultatE1   { get; set; }
		public int ResultatE2	{ get; set; }
		public String Resultat	{ get; set; }

		public String Equipe1 { get; set; }
		public String Equipe2 { get; set; }
		public String GagnantString { get; set; }
		public String ArbitreString { get; set; }



		//REDEFINITION DE LA METHODE TOSTRING   
		public override String ToString()
		{
			return IdRencontre + "." + Phase + "    /EQUIPES : " + E1 + " - " + E2 + "    /Arbitres : " + Arbitre + "    /Table : " + Table +
				  "    /Vainceur :" + Gagnant + "    /Resultat : " + Resultat;
		}


		//Constructeur
		public Rencontre() { }    //par defaut

		public Rencontre(Rencontre r)
		{
			this.IdRencontre = r.IdRencontre;
			this.Phase = r.Phase;
			this.E1 = r.E1;
			this.E2 = r.E2;
			this.Arbitre = r.Arbitre;
			this.Table = r.Table;
			this.Gagnant = r.Gagnant;
			this.Resultat = r.Resultat;
		}

		public Rencontre(int id, String phase, int e1, int e2, int arbitre, int table, int gagnant, String resultat)
		{
			this.IdRencontre = id;
			this.Phase = phase;
			this.E1 = e1;
			this.E2 = e2;
			this.Arbitre = arbitre;
			this.Table = table;
			this.Gagnant = gagnant;
			this.Resultat = resultat;
		}

		public Rencontre(int id, String phase, String e1, String e2, String arbitre, int table, String gagnant, String resultat)
		{
			this.IdRencontre = id;
			this.Phase = phase;
			this.Equipe1 = e1;
			this.Equipe2 = e2;
			this.ArbitreString = arbitre;
			this.Table = table;
			this.GagnantString = gagnant;
			this.Resultat = resultat;
		}

		public Rencontre(int idRencontre, string phase, int e1, int e2, int arbitre, int table, int gagnant, int resultatE1, int resultatE2, string resultat)
		{
			IdRencontre = idRencontre;
			Phase = phase;
			E1 = e1;
			E2 = e2;
			Arbitre = arbitre;
			Table = table;
			Gagnant = gagnant;
			ResultatE1 = resultatE1;
			ResultatE2 = resultatE2;
			Resultat = resultat;
		}
	}
}
