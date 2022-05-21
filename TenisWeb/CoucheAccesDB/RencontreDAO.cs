using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TenisWeb.Models;

namespace TenisWeb.CoucheAccesDB
{
    public class RencontreDAO : BaseDAO<Rencontre>
    {
        public RencontreDAO(SqlCommand sqlCmd) : base(sqlCmd) { }


		/**
		 * Methode qui ajoute une equipe dans la DB 
		 * @param obj : est une equipe
		 */
		public override void Ajouter(Rencontre obj) 
		{
			try
			{
				int numRencontre;

				SqlCmd.CommandText = "select max(idRencontre) + 1 FROM RENCONTRE ";

				SqlCmd.Parameters.Clear();

				SqlDataReader sqlReader = SqlCmd.ExecuteReader();

				sqlReader.Read();

				if (sqlReader[0] == DBNull.Value)
					numRencontre = 1;
				else
					numRencontre = Convert.ToInt32(sqlReader[0]);

				sqlReader.Close();

				SqlCmd.CommandText = "INSERT INTO RENCONTRE(idRencontre, phase, idEquipe1, idEquipe2, idArbitre, idTable, idGagnant, resultat)" +
										"VALUES(@IdRencontre, @Phase, @E1, @E2, @Arbitre, @Table, @Gagnant, @Resultat)";

				SqlCmd.Parameters.Clear();

				SqlCmd.Parameters.Add("@IdRencontre", SqlDbType.Int).Value = numRencontre;
				SqlCmd.Parameters.Add("@Phase"		, SqlDbType.VarChar).Value = obj.Phase;
				SqlCmd.Parameters.Add("@E1"			, SqlDbType.Int).Value = obj.E1;
				SqlCmd.Parameters.Add("@E2"			, SqlDbType.Int).Value = obj.E2;
				SqlCmd.Parameters.Add("@Arbitre"	, SqlDbType.Int).Value = obj.Arbitre;
				SqlCmd.Parameters.Add("@Table"		, SqlDbType.Int).Value = obj.Table;
				SqlCmd.Parameters.Add("@Gagnant"	, SqlDbType.Int).Value = obj.Gagnant;
				SqlCmd.Parameters.Add("@Resultat"	, SqlDbType.VarChar).Value = obj.Resultat;

				SqlCmd.ExecuteNonQuery();

			}
			catch(Exception e)
			{
			throw new ExceptionAccesDB(e.Message);
			}
		}


		/**
		 * Methode qui met ajour une rencontre de la DB
		 */
		public override void Modifier(Rencontre obj)
		{
			try
			{
				SqlCmd.CommandText = "UPDATE RENCONTRE "
									+"SET   phase = @Phase, idEquipe1 = @E1, idEquipe2 = @E2, idArbitre = @Arbitre, idTable = @Table, idGagnant = @Gagnant, resultat = @Resultat "
									+"WHERE idRencontre = @IdRencontre";


				
				SqlCmd.Parameters.Add("@Phase", SqlDbType.VarChar).Value = obj.Phase;
				SqlCmd.Parameters.Add("@E1", SqlDbType.Int).Value = obj.E1;
				SqlCmd.Parameters.Add("@E2", SqlDbType.Int).Value = obj.E2;
				SqlCmd.Parameters.Add("@Arbitre", SqlDbType.Int).Value = obj.Arbitre;
				SqlCmd.Parameters.Add("@Table", SqlDbType.Int).Value = obj.Table;
				SqlCmd.Parameters.Add("@Gagnant", SqlDbType.Int).Value = obj.Gagnant;
				SqlCmd.Parameters.Add("@Resultat", SqlDbType.VarChar).Value = obj.Resultat;

				SqlCmd.Parameters.Add("@IdRencontre", SqlDbType.Int).Value = obj.IdRencontre;

				SqlCmd.ExecuteNonQuery();
			}
			catch(Exception e)
			{
				throw new ExceptionAccesDB(e.Message);
			}
		}



		/**
		 * Methode qui supprime toutes les rencontre  de la base de donne
		 */
		public void SupprimerTous() 
		{
			try
			{
				SqlCmd.CommandText = "DELETE FROM RENCONTRE ";

				SqlCmd.Parameters.Clear();
				
				SqlCmd.ExecuteNonQuery();
			}
			catch(Exception e)
			{
				throw new ExceptionAccesDB(e.Message);
			}
		}

		/**
		 * Methode qui lit dans la DB un rencontre specifique
		 * @param num : le numero de la rencontre
		 */
		public override Rencontre Charger(int num) 
		{
			Rencontre rencontre = null;
		
			try
			{
				SqlCmd.CommandText = "SELECT idRencontre, phase, idEquipe1, idEquipe2, idArbitre, idTable, idGagnant, resultat "
									+ "FROM  RENCONTRE "
									+ "WHERE idRencontre = @IdRencontre ";

				SqlCmd.Parameters.Clear();

				SqlCmd.Parameters.Add("@IdRencontre", SqlDbType.Int).Value = num;

				SqlDataReader sqlReader = SqlCmd.ExecuteReader();

				if (sqlReader.Read() == true)
				{
					rencontre = new Rencontre(	Convert.ToInt32(sqlReader["idRencontre"]),
												Convert.ToString(sqlReader["phase"]),
												Convert.ToInt32(sqlReader["idEquipe1"]),
												Convert.ToInt32(sqlReader["idEquipe2"]),
												Convert.ToInt32(sqlReader["idArbitre"]),
												Convert.ToInt32(sqlReader["idTable"]),
												Convert.ToInt32(sqlReader["idGagnant"]),
												Convert.ToString(sqlReader["resultat"]));
				}
				sqlReader.Close();

				return rencontre;
			}
			catch(Exception e)
			{
				throw new ExceptionAccesDB(e.Message);
			}
		}

		/**
		 * Methode qui lit dans la DB les rencontre d'une equipe
		 * @param num : numero de l'equipe
		 */
		public override List<Rencontre> ChargerTous(int num) 
		{
			List<Rencontre> list = new List<Rencontre>();
		
			try
			{
				SqlCmd.CommandText = "SELECT idRencontre, phase, idEquipe1, idEquipe2, idArbitre, idTable, idGagnant, resultat "
									+"FROM  RENCONTRE "
									+"WHERE idEquipe1 = @IdEquipe OR idEquipe2 = @IdEquipe ";

				SqlCmd.Parameters.Clear();

				SqlCmd.Parameters.Add("@IdEquipe", SqlDbType.Int).Value = num;

				SqlDataReader sqlReader = SqlCmd.ExecuteReader();

				while (sqlReader.Read() == true)
				{
					
						list.Add(new Rencontre(Convert.ToInt32(sqlReader["idRencontre"]),
												Convert.ToString(sqlReader["phase"]),
												Convert.ToInt32(sqlReader["idEquipe1"]),
												Convert.ToInt32(sqlReader["idEquipe2"]),
												Convert.ToInt32(sqlReader["idArbitre"]),
												Convert.ToInt32(sqlReader["idTable"]),
												Convert.ToInt32(sqlReader["idGagnant"]),
												Convert.ToString(sqlReader["resultat"])));
					
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
		 * Methode qui lit dans la DB toutes les rencontres
		 */
		public override List<Rencontre> ListerTous() 
		{
			List<Rencontre> list = new List<Rencontre>();
		
			try
			{
				SqlCmd.CommandText = "SELECT idRencontre, phase, idEquipe1, idEquipe2, idArbitre, idTable, idGagnant, resultat "
									+"FROM RENCONTRE ";

				SqlCmd.Parameters.Clear();

				SqlDataReader sqlReader = SqlCmd.ExecuteReader();

				while (sqlReader.Read() == true)
				{
					list.Add(new Rencontre( Convert.ToInt32(sqlReader["idRencontre"]),
											Convert.ToString(sqlReader["phase"]),
											Convert.ToInt32(sqlReader["idEquipe1"]),
											Convert.ToInt32(sqlReader["idEquipe2"]),
											Convert.ToInt32(sqlReader["idArbitre"]),
											Convert.ToInt32(sqlReader["idTable"]),
											Convert.ToInt32(sqlReader["idGagnant"]),
											Convert.ToString(sqlReader["resultat"])));
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
