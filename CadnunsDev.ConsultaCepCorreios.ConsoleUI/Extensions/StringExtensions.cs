using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CadnunsDev.ConsultaCepCorreios.ConsoleUI.Extensions
{
    public static class StringExtensions
    {
        public static string HtmlDecode(this string text)
        {
            return WebUtility.HtmlDecode(text);
        }
    }
}
