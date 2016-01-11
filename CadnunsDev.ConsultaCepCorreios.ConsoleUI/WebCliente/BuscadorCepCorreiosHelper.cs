using CadnunsDev.ConsultaCepCorreios.Domain.Models;
using CadnunsDev.ConsultaCepCorreios.ConsoleUI.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CadnunsDev.ConsultaCepCorreios.ConsoleUI.WebCliente
{
    public class BuscadorCepCorreiosHelper
    {
        private string _linkPage;
        private string _buscarCepPeloLogradouroLink;
        private string _buscarLogradouroPeloCepLink;

        public BuscadorCepCorreiosHelper(string linkPage)
        {
            // TODO: Complete member initialization
            this._linkPage = linkPage;
        }

        public BuscadorCepCorreiosHelper()
        {
            _buscarCepPeloLogradouroLink = "http://www.buscacep.correios.com.br/sistemas/buscacep/resultadoBuscaCep.cfm";
            _buscarLogradouroPeloCepLink = "http://www.buscacep.correios.com.br/sistemas/buscacep/resultadoBuscaCepEndereco.cfm";
        }

        public Logradouro GerarLogradouro(string cepDesejado, string tipoCEP = "ALL", string semelhante = "N")
        {
            var postData = string.Format("relaxation={0}&tipoCEP={1}&semelhante={2}", cepDesejado, tipoCEP, semelhante);
            
            var responseString = GetHtml(postData, _buscarLogradouroPeloCepLink);

            var pattern = @"<table class=""tmptabela"">(.*?)</table>";
            var regex = new Regex(pattern);
            var match = regex.Match(responseString);

            var rua = new Regex("<td width=\"150\">(.*?)&nbsp;</td>").Match(match.Groups[0].Value).Groups[0].Value;
            string stripTagsPattern = @"<(.|\n)*?>";
            rua = Regex.Replace(rua, stripTagsPattern, string.Empty).Replace("&nbsp;","");

            var bairro = new Regex("<td width=\"90\">(.*?)&nbsp;</td>").Match(match.Groups[0].Value).Groups[0].Value;
            bairro = Regex.Replace(bairro, stripTagsPattern, string.Empty).Replace("&nbsp;", "");

            var cidade = new Regex("<td width=\"80\">(.*?)</td>").Match(match.Groups[0].Value).Groups[0].Value;
            cidade = Regex.Replace(cidade, stripTagsPattern, string.Empty).Replace("&nbsp;", "");

            var logradouro = new Logradouro();
            logradouro.CEP = cepDesejado;
            logradouro.Endereco = rua.HtmlDecode();
            logradouro.BairroOuDistrito = bairro.HtmlDecode();
            logradouro.Localidade = cidade.Split('/')[0].HtmlDecode();
            logradouro.UF = cidade.Split('/')[1].HtmlDecode();
            return logradouro;
        }

        public string BuscarCep(string uf, string localidade, string tipo, string logradouro, string numero)
        {
            var postData = string.Format("UF={0}&Localidade={1}&Tipo={2}&Logradouro={3}&Numero={4}", uf, localidade, tipo, logradouro, numero);

            var responseString = GetHtml(postData, _buscarCepPeloLogradouroLink);

            var pattern = @"<table class=""tmptabela"">(.*?)</table>";
            var regex = new Regex(pattern);
            var match = regex.Match(responseString);

            return "";
        }

        private string GetHtml(string postData, string linkPage)
        {
            var request = (HttpWebRequest)WebRequest.Create(linkPage);

            //var postData = "thing1=hello";
            //postData += "&thing2=world";
            
            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            return new StreamReader(response.GetResponseStream()).ReadToEnd();

        }
    }
}
