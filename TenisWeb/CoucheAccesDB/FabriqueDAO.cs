using System;
using System.Data.SqlClient;
namespace TenisWeb.CoucheAccesDB
{
    public class FabriqueDAO
    {
		//variable stockant la connexion a la DB
		private SqlCommand SqlCmd;

		//Constructeur : il cree la connexion a la DB
		 public FabriqueDAO() 
		{
			try
			{
				SqlCmd = new SqlCommand();
				SqlCmd.Connection = new SqlConnection("Integrated security=false;" +
														/*"user id=genial;" +
														"password=super;" +*/
														"Data Source=RAMZAN-NOXCHO\\SQLEXPRESS;" +
														"Initial Catalog=Tournoi2;");
				SqlCmd.Connection.Open();
			}
			catch(Exception e)
			{
				throw new ExceptionAccesDB(e.Message);
			}
		}


		/**
		 * Methode qui debute explicitement une transaction
		 */
		public void DebuterTransaction() 
		{
			SqlCmd.Transaction = SqlCmd.Connection.BeginTransaction();
		}

		/**
		 * Methode qui valide explicitement une transaction courante
		 */
		public void ValiderTransaction() 
		{
			SqlCmd.Transaction.Commit();
		}

		/**
		 * Methode qui annule explicitement une transaction courante
		 */
		public void AnnulerTransaction() 
		{
			SqlCmd.Transaction.Rollback();
		}

		/**
		 * Methode qui fourni une instance d'ARBITRE
		 * @return instance ARBITRE
		 */
		public ArbitreDAO GetArbitreDAO()
		{
			return new ArbitreDAO(SqlCmd);
		}

		/**
		 * Methode qui fourni une instance d'EQUIPE
		 * @return instance EQUIPE
		 */
		public EquipeDAO GetEquipeDAO()
		{
			return new EquipeDAO(SqlCmd);
		}

		/**
		 * Methode qui fourni une instance d'JOUEUR
		 * @return instance JOUEUR
		 */
		public JoueurDAO GetJoueurDAO()
		{
			return new JoueurDAO(SqlCmd);
		}

		/**
		 * Methode qui fourni une instance d'RENCONTRE
		 * @return instance RENCONTRE
		 */
		public RencontreDAO GetRencontreDAO()
		{
			return new RencontreDAO(SqlCmd);
		}

		/**
		 * Methode qui fourni une instance d'TABLE
		 * @return instance TABLE
		 */
		public TableDAO GetTableDAO()
		{
			return new TableDAO(SqlCmd);
		}
    }
}
