using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuakeLogParser.Model
{
    public class PlayerKill
    {
        public Player player { get; set; }
        public int kills { get; set; }

        public PlayerKill()
        {
            this.player = new Player();
            this.kills = 0;
        }
    }
}
