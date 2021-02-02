using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wolftrack.Models;

namespace wolftrack.DTO
{
    public class AddPackDTO
    {
        public string Name { get; set; }
        public List<Wolf> Wolves { get; set; }
        
    }
}
