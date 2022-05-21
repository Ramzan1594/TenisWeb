using System;

namespace TenisWeb.coucheMetier
{
    public class ExceptionMetier : Exception
    {
        public ExceptionMetier(string message) : base(message) { }
    }
}
