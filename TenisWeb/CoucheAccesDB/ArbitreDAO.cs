using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TenisWeb.Models;


namespace TenisWeb.CoucheAccesDB
{
    public class ArbitreDAO : BaseDAO<Arbitre>
    {

		/**
		 * Constructeur
		 * param sqlCmd : la commande SQL contenant la connexion a la DB
		 **/
		public ArbitreDAO(SqlCommand sqlCmd) : base(sqlCmd) {}

		/**
		 * Methode qui lit dans la DB un arbitre specifique
		 * @param num : le numero de l'arbitre
		 * return l'arbitre
		 */
		public override Arbitre Charger(int num)
		{
			Arbitre arbitre = null;
		
			try
			{
				SqlCmd.CommandText = "SELECT idArbitre, nom, prenom, anneExperience "
											+ "FROM  ARBITRE "
											+ "WHERE idArbitre = @IdArbitre ";
				SqlCmd.Parameters.Clear();

				SqlCmd.Parameters.Add("@IdArbitre", SqlDbType.Int).Value = num;

				SqlDataReader sqlReader = SqlCmd.ExecuteReader();

				if (sqlReader.Read() == true)
				{
					arbitre = new Arbitre(  Convert.ToInt32(sqlReader["idArbitre"]),
											Convert.ToString(sqlReader["nom"]),
											Convert.ToString(sqlReader["prenom"]),
											Convert.ToInt32(sqlReader["anneExperience"]) );
				}
				sqlReader.Close();
				return arbitre;
			}
			catch(Exception e)
			{
				throw new ExceptionAccesDB(e.Message);
			}			
		}

		/**
		 * Methode qui lit tous les arbitres de la DB
		 * return : la liste des arbitres
		 * */
		public override List<Arbitre> ListerTous() 
		{
			List<Arbitre> list = new List<Arbitre>();
		
			try
			{
				SqlCmd.CommandText =  "SELECT idArbitre, nom, prenom, anneExperience "
											+ "FROM ARBITRE "
											+ "ORDER BY idArbitre ASC ";

				SqlCmd.Parameters.Clear();

				SqlDataReader sqlReader = SqlCmd.ExecuteReader();


				while (sqlReader.Read() == true)
				{
					list.Add(new Arbitre(Convert.ToInt32(sqlReader["idArbitre"]),
										 Convert.ToString(sqlReader["nom"]),
										 Convert.ToString(sqlReader["prenom"]),
										 Convert.ToInt32(sqlReader["anneExperience"])));
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
