using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TenisWeb.Models;


namespace TenisWeb.CoucheAccesDB
{
    public class EquipeDAO : BaseDAO<Equipe>
	{

		public EquipeDAO(SqlCommand sqlCmd) : base(sqlCmd) { }

		/**
		 * Methode qui ajoute une equipe dans la DB 
		 * @param obj : est une equipe
		 */
		public override void Ajouter(Equipe obj) 
		{
			try
			{
				int numEquipe;

				SqlCmd.CommandText = "select max(idEquipe) + 1 FROM EQUIPE ";

				SqlCmd.Parameters.Clear();

				SqlDataReader sqlReader = SqlCmd.ExecuteReader();

				sqlReader.Read();


				if (sqlReader[0] == DBNull.Value)
					numEquipe = 1;
				else
					numEquipe = Convert.ToInt32(sqlReader[0]);

				sqlReader.Close();

				SqlCmd.CommandText = "INSERT INTO EQUIPE(idEquipe, nom, idJoueur1, idJoueur2) " +
									 "VALUES(@IdEquipe, @Nom, @J1, @J2) ";

				SqlCmd.Parameters.Clear();

				SqlCmd.Parameters.Add("@IdEquipe",  SqlDbType.Int).Value = numEquipe;
				SqlCmd.Parameters.Add("@Nom",		SqlDbType.VarChar).Value = obj.Nom;
				SqlCmd.Parameters.Add("@J1",		SqlDbType.Int).Value = obj.J1;
				SqlCmd.Parameters.Add("@J2",		SqlDbType.Int).Value = obj.J2;

				SqlCmd.ExecuteNonQuery();
			}
			catch(Exception e)
			{
				throw new ExceptionAccesDB(e.Message);
			}
		}

		/**
		 * Methode qui supprimer une equipe de la base de donne
		 * @param num    id de l'equipe a supprimer
		 */
		public void SupprimerEquipe(int num) 
		{
			try
			{
				SqlCmd.CommandText = "DELETE FROM EQUIPE WHERE idEquipe = @IdEquipe";

				SqlCmd.Parameters.Clear();

				SqlCmd.Parameters.Add("@IdEquipe", SqlDbType.Int).Value = num;

				SqlCmd.ExecuteNonQuery();
			}
			catch(Exception e)
			{
				throw new ExceptionAccesDB(e.Message);
			}
		}


		/**
		 * Methode qui lit dans la DB une equipe specifique
		 * @param num : le numero de la table
		 */
		public override Equipe Charger(int num) 
		{
			Equipe equipe = null;
		
			try
			{
				SqlCmd.CommandText = "SELECT idEquipe, nom, idJoueur1, idJoueur2 "
									+ "FROM  EQUIPE "
									+ "WHERE idEquipe = @IdEquipe ";
				SqlCmd.Parameters.Clear();

				SqlCmd.Parameters.Add("@IdEquipe", SqlDbType.Int).Value = num;

				SqlDataReader sqlReader = SqlCmd.ExecuteReader();
				
				if (sqlReader.Read() == true)
				{
					equipe = new Equipe(Convert.ToInt32(sqlReader["idEquipe"]),
										Convert.ToString(sqlReader["nom"]),
										Convert.ToInt32(sqlReader["idJoueur1"]),
										Convert.ToInt32(sqlReader["idJoueur2"]));
				}

				sqlReader.Close();
				return equipe;
			}
			catch(Exception e)
			{
				throw new ExceptionAccesDB(e.Message);
			}			
		}

		/**
		 * Methode qui lit dans la DB toutes les equipes
		 */
		public override List<Equipe> ListerTous() 
		{
			List<Equipe> list = new List<Equipe>();
		
			try
			{
				SqlCmd.CommandText = "SELECT idEquipe, nom, idJoueur1, idJoueur2 "
									+ "FROM EQUIPE "
									+ "ORDER BY idEquipe ASC ";
				SqlCmd.Parameters.Clear();

				SqlDataReader sqlReader = SqlCmd.ExecuteReader();

				while (sqlReader.Read() == true)
				{
					list.Add(new Equipe(Convert.ToInt32(sqlReader["idEquipe"]),
										Convert.ToString(sqlReader["nom"]),
										Convert.ToInt32(sqlReader["idJoueur1"]),
										Convert.ToInt32(sqlReader["idJoueur2"])));
				}
				sqlReader.Close();
			
			}
			catch(Exception e)
			{
				throw new ExceptionAccesDB(e.Message);
			}
			return list;
		}
	
	
		/**
		 * Methode qui lit dans la DB l'equipe qui contient un joueur specifique, pour voir s'il est deja dans une equipe
		 * @param num  est le nombre d'equipe qui contienne un des deux joueurs
		 */
		public int ListerEquipeSelonJoueur(int j1, int j2) 
		{
			List<Equipe> list = new List<Equipe>();
			int equipe = 0;
		
			try
			{
				SqlCmd.CommandText = "SELECT idEquipe, nom, idJoueur1, idJoueur2 "
									+ "FROM EQUIPE "
									+ "WHERE idJoueur1 = @J1 OR idJoueur1 = @J2 OR idJoueur2 = @J1 OR idJoueur2 = @J2 ";
				SqlCmd.Parameters.Clear();

				SqlCmd.Parameters.Add("@J1", SqlDbType.Int).Value = j1;
				SqlCmd.Parameters.Add("@J2", SqlDbType.Int).Value = j2;

				SqlDataReader sqlReader = SqlCmd.ExecuteReader();
			
				while(sqlReader.Read() == true)
				{
					list.Add(new Equipe(Convert.ToInt32(sqlReader["idEquipe"]),
										Convert.ToString(sqlReader["nom"]),
										Convert.ToInt32(sqlReader["idJoueur1"]),
										Convert.ToInt32(sqlReader["idJoueur2"])));
				}
				sqlReader.Close();
			}
			catch(Exception e)
			{
				throw new ExceptionAccesDB(e.Message);
			}
			equipe = list.Count;
		
			return equipe;
		}


		/**
		 * Quand on veut supprimer un joeur il faut d'abord supprimer l'equipe 
		 * dans laquelle le joueur se trouve, cette methode supprime l'equipe 
		 * du joueur
		 * @param numJoueur numJoueur a rechercher
		 */
		public void Supprimer(int numJoueur) 
		{
			try
			{
				SqlCmd.CommandText = "DELETE FROM EQUIPE WHERE idJoueur1 = @IdJoueur OR idJoueur2 = @IdJoueur";
				SqlCmd.Parameters.Clear();

				SqlCmd.Parameters.Add("@IdJoueur", SqlDbType.Int).Value = numJoueur;

				SqlCmd.ExecuteNonQuery();
			}
			catch(Exception e)
			{
				throw new ExceptionAccesDB(e.Message);
			}
		}
    }
}
