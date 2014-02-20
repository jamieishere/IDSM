using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDSM.Repository.DTOs
{
    public class GameUpdateDto
    {
        public int Id { get; set; }
        public int CreatorId { get; set; }
        public string Name { get; set; }
        public int CurrentOrderPosition { get; set; }
        public bool HasStarted { get; set; }
        public bool HasEnded { get; set; }
        public int WinnerId { get; set; }
    }
}
