﻿using IDSM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDSM.Repository
{
    public class FakePlayerRepository : RepositoryBase<IDSMContext>, IPlayerRepository
    {
        public Player GetPlayer(int id)
        {
            return new Player
            {
                Id = id,
                Name = "Wayne Rooney"
            };
        }

        public IEnumerable<Player> GetAllPlayers()
        {
            return new List<Player>
            {
                new Player {
                Id = 1,
                Name = "Ryan Giggs"
                },
                new Player {
                Id = 2,
                Name = "Christiano Ronaldo"
                }
            };
        }

        public IEnumerable<string> GetAllClubs()
        {
            return new List<string>
            {
                "Manchester United",
                "Liverpool"
            };
        }
    }
}
