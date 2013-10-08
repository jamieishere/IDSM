using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IDSM.Model;

namespace IDSM.Helpers
{
    public class PlayerComparer : IEquatable<Player>
    {

        public int Id;
       // public string Name;

        public PlayerComparer(int Id)
        {
            this.Id = Id;
        }

        public bool Equals(Player other)
        {
            if (other == null) return false;
            return (this.Id.Equals(other.Id));
        }

        // Nested class
        //private class playerIDHelper: IComparer
        //{
        //    bool IComparer.Compare(object a, object b)
        //    {
        //        Player p1 = (Player)a;
        //        Player p2 = (Player)b;
        //        if (p1.Id == p2.Id)
        //            return true;
        //        else
        //            return false;
        //    }
        //}

        //private Player p1;
        //private Player p2;
        
        //public PlayerComparer(Player P1, Player P2)
        //{
        //    p1=P1;
        //    p2=P2;
        //}


    }
}