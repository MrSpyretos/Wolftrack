using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wolftrack.Models
{
    public class Pack
    {
        public int PackId { get; set; }
        public string Name{ get; set; }
        public List<Wolf> Wolves { get; set; }
        public Pack()
        {
            Wolves = new List<Wolf>();
        }
    }
}
