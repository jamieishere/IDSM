using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDSM.Exceptions
{
    [Serializable]
    public class UserTeamException : Exception
    {
        public UserTeamException()
        {
        }
        public UserTeamException(string message) : base(message)
        {
        }
        public UserTeamException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

}