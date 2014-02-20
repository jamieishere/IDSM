using System;

namespace IDSM.Model
{
    public class Banter
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int UserTeamId { get; set; }
        //public int UserId { get; set; }
        //public int GameId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string BanterText { get; set; }
        public int Votes { get; set; }
    }
}
