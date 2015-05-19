using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tesis_ClienteWeb_Data.UserExceptions
{
    public class SessionExpiredException : Exception
    {
        public SessionExpiredException()
        {

        }

        public SessionExpiredException(string message) : base(message)
        {

        }

        public SessionExpiredException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}