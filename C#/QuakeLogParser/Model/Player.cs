using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuakeLogParser.Model
{
    public class Player
    {
        public int id { get; set; }
        public string nome { get; set; }

        public Player()
        {
            this.id = 0;
            this.nome = String.Empty;
        }
    }
}
