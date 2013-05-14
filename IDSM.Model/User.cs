//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace IDSM.Model
//{
//    ///<summary>
//    /// User model
//    /// A user contains all information related to an individual user of IDSM
//    ///</summary>
//    ///<remarks>
//    ///This is a test.
//    ///</remarks>
//    public class User
//    {
//        //constructor that populates the navigation properties
//        public User()
//        {
//            UserTeams = new HashSet<UserTeam>();
//        }

//        //primitive properties
//        public int Id { get; set; }
//        public string Name { get; set; }

//        //navigation properties
//        public virtual ICollection<UserTeam> UserTeams { get; set; }
//        [ForeignKey("CreatorId")]
//        public virtual ICollection<Game> Games { get; set; }
        
//        //need to watch the videos

//        //navigation properties
//        //http://blog.staticvoid.co.nz/2012/7/17/entity_framework-navigation_property_basics_with_code_first
//        // thing is - in account at a glance, he seems to use IDs in some situations not navigation propertoes... why?
//        // maybe like i need for game?  like a user has many user teams.. and a game has many user teams.. NOPE

//        //navigation properties - should these be virtual? to be overridden?
//        // this is how foreign key relationships are done with EF...   but not entirely sure if both this & one in users should be virtual
//        // also no sure if need to specify  foregin key, or if it works that out itself. doesnt appear to be in account at a glance, but then they did model first.

//        // also, in account at a glance - for instance of brokerageaccount/position - they use 
//        //public ICollection<Position> Positions { get; set; }  in brokerageaccount
//        // but in positions only
//        //        public int BrokerageAccountId { get; set; }
//        // so dont use an instance of the brokerageaccoutn... only the id...
//        // confused..


//        //http://stackoverflow.com/questions/5542864/how-should-i-declare-foreign-key-relationships-using-code-first-entity-framework
//        //http://stackoverflow.com/questions/13823922/mvc-model-foreign-key-navigational-property
//        //http://msdn.microsoft.com/en-us/library/ee382841.aspx
//        //http://stackoverflow.com/questions/12678833/c-sharp-when-to-use-public-int-virtual-and-when-to-just-use-public-int
//        //http://msdn.microsoft.com/en-us/library/ms173152.aspx
//        //http://stackoverflow.com/questions/5446316/ef-4-1-code-first-vs-model-database-first

//        // further reading - lazy/eager loading
//        //http://stackoverflow.com/questions/5466384/how-to-work-with-navigation-properties-foreign-keys-in-asp-net-mvc-3-and-ef-4



//    }
//}
