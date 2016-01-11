using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadnunsDev.ConsultaCepCorreios.Domain.Models
{
    public class Logradouro
    {
        public string Nome { get; set; }
        public string BairroOuDistrito { get; set; }
        public string Localidade { get; set; }
        public string UF { get; set; }
        public string CEP { get; set; }
    }
}
