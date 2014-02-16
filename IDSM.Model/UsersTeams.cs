using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDSM.Model
{

    public class UsersTeams
    {
        //constructor 
        public UsersTeams()
        {
            AllUsersTeams = new HashSet<UserTeam>();
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public UserProfile User { get; set; }
        public ICollection<UserTeam> AllUsersTeams { get; set; }
    }
}
