using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using wolftrack.Models;

namespace wolftrack.DTO
{
    public class UpdateWolfDTO
    {
        public int WolfId { get; set; }
        [StringLength(10, MinimumLength = 2)]
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Birthday { get; set; }
        public string Location { get; set; }
        public Pack Pack { get; set; }
    }
}
