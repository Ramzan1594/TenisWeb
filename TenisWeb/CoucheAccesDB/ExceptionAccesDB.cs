using System;

namespace TenisWeb.CoucheAccesDB
{
    public class ExceptionAccesDB : Exception
    {
        public ExceptionAccesDB(string message) : base(message) { }
    }
}
