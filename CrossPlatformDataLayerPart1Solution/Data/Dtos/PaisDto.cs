using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Dtos
{
    public class PaisDto
    {
        public int PaisId { get; set; } // NOT NULL
        public string ISO2 { get; set; } // NULL
        public string ISO3 { get; set; } // NULL
        public int? NumCode { get; set; } // NULL
        public string Nome { get; set; } // NOT NULL
        public string DDI { get; set; } // NULL
    }
}
