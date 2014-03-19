using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuakeLogParser.Model
{
    public class Game
    {
        public int totalKills { get; set; }
        public List<Player> players { get; set; }
        public List<PlayerKill> playersKills { get; set; }
        public List<KillsByMean> killsByMeans { get; set; }
    }
}
