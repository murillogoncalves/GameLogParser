using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuakeLogParser.Enum;

namespace QuakeLogParser.Model
{
    public class KillsByMean
    {
        public eMeansOfDeath mean { get; set; }
        public int numero { get; set; }

        public KillsByMean()
        {
            
        }
    }
}
