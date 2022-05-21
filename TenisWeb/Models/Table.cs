using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenisWeb.Models
{
    public class Table
    {
		//Propriete get set pour avoir/attribue les valeurs aux variables
		public int IdTable { get; set; }
		public int Position { get; set; }

		
		//REDEFINITION DE LA METHODE TOSTRING   
		public override String ToString() { return IdTable + " " + Position; }


		//Constructeur
		public Table() { }    //par defaut

		public Table(Table t)
		{
			this.IdTable = t.IdTable;
			this.Position = t.Position;
		}

		public Table(int id, int position)
		{
			this.IdTable = id;
			this.Position = position;
		}
	}
}
