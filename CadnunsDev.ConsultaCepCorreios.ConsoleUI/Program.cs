using CadnunsDev.ConsultaCepCorreios.ConsoleUI.WebCliente;
using CadnunsDev.ConsultaCepCorreios.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CadnunsDev.ConsultaCepCorreios.ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            var webCliente = new WebClientHelper("http://www.buscacep.correios.com.br/sistemas/buscacep/resultadoBuscaCepEndereco.cfm");
            var cepDesejado = "08130-050";
            Console.WriteLine("Digite o cep desejado");

            cepDesejado = Console.ReadLine();

            if (Regex.IsMatch(cepDesejado, @"\d{5}\-\d{3}"))
            {
                Logradouro logradouro = webCliente.GerarLogradouro(cepDesejado);
                Console.WriteLine("Cep.: {0}, End.:{1} Bairro: {2} - {3}/{4}", logradouro.CEP, logradouro.Endereco, logradouro.BairroOuDistrito, logradouro.Localidade, logradouro.UF);
            }
            

            Console.Read();
        }
    }
}
