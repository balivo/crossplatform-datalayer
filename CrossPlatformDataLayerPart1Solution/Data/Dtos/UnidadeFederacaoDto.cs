using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Dtos
{
    public class UnidadeFederacaoDto
    {
        public int UnidadeFederacaoId { get; set; } // NOT NULL
        public int PaisId { get; set; } // NOT NULL
        public string Nome { get; set; } // NOT NULL
        public string Sigla { get; set; } // NOT NULL
    }
}
