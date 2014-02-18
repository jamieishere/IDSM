using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Web.WebPages;
using System.Dynamic;
using System.IO;
using System.Threading;

namespace IDSM.ServiceLayer
{

    ///<summary>
    /// ListExtensions
    /// Etension methods for List
    ///</summary>
    ///<remarks>
    ///</remarks>

    static class ListExtensions
    {
        ///<summary>
        /// Shuffle
        /// Randomly shuffles a list order
        ///</summary>
        ///<param name="list"></param>
        ///<remarks>
        ///Rquired to randomly shuffle the UserTeams into an order of play when a game is started.
        ///</remarks>
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

    public static class ThreadSafeRandom
    {
        [ThreadStatic]
        private static Random Local;

        public static Random ThisThreadsRandom
        {
            get { return Local ?? (Local = new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId))); }
        }
    }
}