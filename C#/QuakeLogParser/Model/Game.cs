﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuakeLogParser.Model
{
    public class Game
    {
        private static int ID = 0;

        public int id { get; set; }
        public int totalKills { get; set; }
        public List<Player> players { get; set; }
        public List<PlayerKill> playersKills { get; set; }
        public List<KillsByMean> killsByMeans { get; set; }

        public Game()
        {
            this.id = ++ID;

            this.totalKills = 0;
            this.players = new List<Player>();
            this.playersKills = new List<PlayerKill>();
            this.killsByMeans = new List<KillsByMean>();
        }
    }
}
