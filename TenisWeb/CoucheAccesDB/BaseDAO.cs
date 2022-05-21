using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//using System.Collections.Generic;
using System.Data.SqlClient;

namespace TenisWeb.CoucheAccesDB
{
    public class BaseDAO<T>
    {
		protected SqlCommand SqlCmd;

		/**
		 * Constructeur 
		 * @param sqlCmd : la commande SQL contenant la connexion a la DB
		 */

		public BaseDAO(SqlCommand sqlCmd)
		{
			SqlCmd = sqlCmd;
		}

		/*
		 * methode dont le comportement doit etre definie dans les sous-classe DAO
		 */
		 //on met VIRTUAL pour que la methode puisse etre OVERRIDE dans les classe filles
		public virtual T Charger(int i) {return default(T); }

		public virtual List<T> ChargerTous(int i)  {return null; }

		public virtual void Ajouter(T obj)  { }

		public virtual void Modifier(T obj)  { }

		public virtual void Supprimer(T obj)  { }

		public virtual List<T> ListerTous()  {return null; }
	}
}
