using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDSM.Model
{
    ///<summary>
    /// Player model
    /// Represents each football player in the system.
    ///</summary>
    ///<remarks>
    ///</remarks>
    public class Player
    {
        public int Id { get; set; }
        //[Required(ErrorMessage = "Name is required")]
        //public bool HasBeenChosen { get; set; }
        //[Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        //[Required(ErrorMessage = "Nation is required")]
        public string Nation { get; set; }
        //[Required(ErrorMessage = "Club is required")]
        public string Club { get; set; }
        //[Required(ErrorMessage = "Position is required")]
        public string Position { get; set; }
        //[Required(ErrorMessage = "Age is required")]
        public int Age { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int Acceleration { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int Aggression { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int Agility { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int Anticipation { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int Balance { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int Bravery { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int Composure { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int Concentration { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int Corners { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int Creativity { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int Crossing { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int Decisions { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int Determination { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int Dribbling { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int Finishing { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int FirstTouch { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int Flair { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int Heading { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int Influence { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int Jumping { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int LongShots { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int LongThrows { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int Marking { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int NaturalFitness { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int OffTheBall { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int Pace { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int Passing { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int Penalties { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int Positioning { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int FreeKicks { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int Stamina { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int Strength { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int Tackling { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int Teamwork { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int Technique { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int WorkRate { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int RightFoot { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int LeftFoot { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int GK { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int SW { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int DR { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int DC { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int DL { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int WBR { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int WBL { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int DM { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int MR { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int MC { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int ML { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int AMR { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int AMC { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int AML { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int ST { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int FR { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int CurA { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int Height { get; set; }
        //[Range(1, 20, ErrorMessage = "Acceleration must be between 1 and 20")]
        public int Weight { get; set; }
    }
}
