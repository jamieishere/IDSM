using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDSM.Exceptions
{
    [Serializable]
    public class PlayerException : Exception
    {
        public PlayerException()
        {
        }
        public PlayerException(string message) : base(message)
        {
        }
        public PlayerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class PlayerNotFoundException : PlayerException
    {
        public PlayerNotFoundException()
        {
        }
        public PlayerNotFoundException(string message) : base(message)
        {
        }
        public PlayerNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

}