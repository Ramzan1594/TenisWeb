using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TenisWeb.Models;

namespace TenisWeb.CoucheAccesDB
{
    public class TableDAO : BaseDAO<Table>
    {
        public TableDAO(SqlCommand sqlCmd) : base(sqlCmd) { }


		/**
		* Methode qui lit dans la DB une table specifique
		* @param num : le numero de la table
		 */
		public override Table Charger(int num) 
		{
			Table table = null;
		
			try
			{
				SqlCmd.CommandText = "SELECT idTable, position "
									+ "FROM  TABLEE "
									+ "WHERE idTable = @IdTable ";
				SqlCmd.Parameters.Clear();

				SqlCmd.Parameters.Add("@IdArbitre", SqlDbType.Int).Value = num;

				SqlDataReader sqlReader = SqlCmd.ExecuteReader();

				if (sqlReader.Read() == true)
				{
					table = new Table(	Convert.ToInt32(sqlReader["idTable"]),
										Convert.ToInt32(sqlReader["position"]));
				}

				sqlReader.Close();
				return table;
			}
			catch(Exception e)
			{
				throw new ExceptionAccesDB(e.Message);
			}			
		}

		/**
		 * Methode qui lit dans la DB tous les tables
		 */
		public override List<Table> ListerTous()
		{
			List<Table> list = new List<Table>();
		
			try
			{
				SqlCmd.CommandText = "SELECT idTable, position "
									+ "FROM TABLEE "
									+ "ORDER BY idTable ASC";

				SqlCmd.Parameters.Clear();

				SqlDataReader sqlReader = SqlCmd.ExecuteReader();

				while (sqlReader.Read() == true)
				{
					list.Add(new Table( Convert.ToInt32(sqlReader["idTable"]),
										Convert.ToInt32(sqlReader["position"])));
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
