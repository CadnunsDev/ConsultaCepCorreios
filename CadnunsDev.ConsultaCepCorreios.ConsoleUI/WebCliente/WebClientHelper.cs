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
    public class WebClientHelper
    {
        private string _linkPage;

        public WebClientHelper(string linkPage)
        {
            // TODO: Complete member initialization
            this._linkPage = linkPage;
        }

        public Logradouro GerarLogradouro(string cepDesejado, string tipoCEP = "ALL", string semelhante = "N")
        {
            
            var request = (HttpWebRequest)WebRequest.Create(_linkPage);

            //var postData = "thing1=hello";
            //postData += "&thing2=world";
            var postData = string.Format("relaxation={0}&tipoCEP={1}&semelhante={2}", cepDesejado, tipoCEP, semelhante);
            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

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
    }
}
