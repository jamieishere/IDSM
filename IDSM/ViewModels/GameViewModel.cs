using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IDSM.Model;

namespace IDSM.ViewModel
{
    /// <summary>
    /// GameViewModel
    /// Used for the Games/Index view
    /// Holds all games 
    /// </summary>
    public class GameViewModel
    {
       // public IEnumerable<GameDTO> Games { get; set; }
        public IEnumerable<Game> Games { get; set; }
    }

    public class GameDTO
    {
        public int Id { get; set; }
        public int CreatorId { get; set; }
        public string Name { get; set; }
        public int CurrentOrderPosition { get; set; }
        public bool HasStarted { get; set; }
        public bool HasEnded { get; set; }
        public int WinnerId { get; set; }

        public ICollection<UserTeamDTO> UserTeamsDTO { get; set; }
    }

    public class UserTeamDTO : UserTeam
    {
        public UserProfile User { get; set; }
    }
}