﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuakeLogParser.Model
{
    public class Games
    {
        public List<Game> games { get; set; }

        public Games()
        {
            this.games = new List<Game>();
        }
    }
}
