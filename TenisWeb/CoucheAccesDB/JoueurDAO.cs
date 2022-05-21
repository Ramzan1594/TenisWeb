using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TenisWeb.Models;

namespace TenisWeb.CoucheAccesDB
{
    public class JoueurDAO : BaseDAO<Joueur>
    {
        public JoueurDAO(SqlCommand sqlCmd) : base(sqlCmd) { }
		
		/**
		 * Methode qui ajoute dans la base de donne un joueur
		 * @param obj : joueur
		 */
		public override void Ajouter(Joueur obj) 
		{
			try
			{
				int numJoueur;

				SqlCmd.CommandText = "select max(idJoueur) + 1 FROM JOUEUR ";

				SqlCmd.Parameters.Clear();

				SqlDataReader sqlReader = SqlCmd.ExecuteReader();

				sqlReader.Read();
								 
				if (sqlReader[0] == DBNull.Value)  
					numJoueur = 1;
				else
					numJoueur = Convert.ToInt32(sqlReader[0]);

				sqlReader.Close();


				SqlCmd.CommandText = "INSERT INTO JOUEUR(idJoueur, nom, prenom, styleJeu, photo ) " +
												"VALUES(@IdJoueur, @Nom, @Prenom, @StyleJeu, @Photo)";
				SqlCmd.Parameters.Clear();

				SqlCmd.Parameters.Add("@IdJoueur", SqlDbType.Int).Value = numJoueur;
				SqlCmd.Parameters.Add("@Nom", SqlDbType.VarChar).Value = obj.Nom;
				SqlCmd.Parameters.Add("@Prenom", SqlDbType.VarChar).Value = obj.Prenom;
				SqlCmd.Parameters.Add("@StyleJeu", SqlDbType.VarChar).Value = obj.StyleJeu;
				SqlCmd.Parameters.Add("@Photo", SqlDbType.VarChar).Value = obj.Photo;

				SqlCmd.ExecuteNonQuery();
			}
			catch(Exception e)
			{
				throw new ExceptionAccesDB(e.Message);
			}
		}

		/**
		 * Methode qui supprimer un joueur de la base de donne
		 * @param num    id du joueur a supprimer
		 */
		public void Supprimer(int num) 
		{
			try
			{
				SqlCmd.CommandText = "DELETE FROM JOUEUR WHERE idJoueur = @IdJoueur";

				SqlCmd.Parameters.Clear();

				SqlCmd.Parameters.Add("@IdJoueur", SqlDbType.Int).Value = num;

				SqlCmd.ExecuteNonQuery();
			}
			catch(Exception e)
			{
				throw new ExceptionAccesDB(e.Message);
			}

		}


		/**
		 * Methode qui lit dans la DB un joueur specifique
		 * @param num : le numero de l'arbitre
		 */
		public override Joueur Charger(int num) 
		{
			Joueur joueur = null;
		
			try
			{
				SqlCmd.CommandText = "SELECT idJoueur, nom, prenom, styleJeu, photo "
									+ "FROM  JOUEUR "
									+ "WHERE idJoueur = @IdJoueur ";

				SqlCmd.Parameters.Clear();

				SqlCmd.Parameters.Add("@IdJoueur", SqlDbType.Int).Value = num;

				SqlDataReader sqlReader = SqlCmd.ExecuteReader();

				if (sqlReader.Read() == true)
				{
					joueur = new Joueur(Convert.ToInt32(sqlReader["idJoueur"]),
										Convert.ToString(sqlReader["nom"]),
										Convert.ToString(sqlReader["prenom"]),
										Convert.ToString(sqlReader["styleJeu"]),
										Convert.ToString(sqlReader["photo"]));
				}

				sqlReader.Close();
				return joueur;
			}
			catch(Exception e)
			{
				throw new ExceptionAccesDB(e.Message);
			}
		}

		/**
		 * Methode qui lit dans la DB tous les joueurs
		 */
		public override List<Joueur> ListerTous() 
		{
			List<Joueur> list = new List<Joueur>();
		
			try
			{
				SqlCmd.CommandText = "SELECT idJoueur, nom, prenom, styleJeu, photo "
									+ "FROM JOUEUR "
									+ "ORDER BY idJoueur ASC ";

				SqlCmd.Parameters.Clear();

				SqlDataReader sqlReader = SqlCmd.ExecuteReader();

				while (sqlReader.Read() == true)
				{
					list.Add(new Joueur(Convert.ToInt32(sqlReader["idJoueur"]),
										Convert.ToString(sqlReader["nom"]),
										Convert.ToString(sqlReader["prenom"]),
										Convert.ToString(sqlReader["styleJeu"]),
										Convert.ToString(sqlReader["photo"])));
				}
				sqlReader.Close();

			}
			catch(Exception e)
			{
				throw new ExceptionAccesDB(e.Message);
			}		
			return list;
		}
    }
}
