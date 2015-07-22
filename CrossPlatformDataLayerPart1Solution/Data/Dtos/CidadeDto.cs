using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Dtos
{
    public class CidadeDto
    {
        public int CidadeId { get; set; } // NOT NULL
        public int UnidadeFederacaoId { get; set; } // NOT NULL
        public string Nome { get; set; } // NOT NULL
        public string DDD { get; set; } // NULL
        public string CEPInicial { get; set; } // NULL
        public string CEPFinal { get; set; } // NULL
        public string Classe { get; set; } // NULL
    }
}
