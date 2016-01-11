using CadnunsDev.ConsultaCepCorreios.ConsoleUI.WebCliente;
using CadnunsDev.ConsultaCepCorreios.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadnunsDev.ConsultaCepCorreios.ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            var webCliente = new WebClientFunctions("http://www.buscacep.correios.com.br/sistemas/buscacep/resultadoBuscaCepEndereco.cfm");
            var relaxation = "08130-050";
            var tipoCEP = "ALL";
            var semelhante = "N";
            Logradouro logradouro = webCliente.GerarLogradouro(relaxation, tipoCEP, semelhante);
            Console.Read();
        }
    }
}
