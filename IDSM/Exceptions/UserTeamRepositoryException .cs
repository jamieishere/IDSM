using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDSM.Exceptions
{
    [Serializable]
    public class UserTeamRepositoryException : Exception
    {
       public string BespokeMessage { get; set; }
       
    }
}