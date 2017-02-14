using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace FutebolTempoRealApp.Model
{
    public static class HtmlRemoval
    {
        /// <summary>
        /// Remove HTML from string with Regex.
        /// </summary>
        public static string StripTagsRegex(string source)
        {
            return Regex.Replace(source, "<.*?>", string.Empty);
        }

        public static string RemoveHTML(string strHTML)
        {
            if (strHTML == null) return "";
            return Regex.Replace(strHTML, "<(.|\n)*?>", "");
        }
        /// <summary>
        /// Compiled regular expression for performance.
        /// </summary>
        static Regex _htmlRegex = new Regex("<.*?>", RegexOptions.None);

        /// <summary>
        /// Remove HTML from string with compiled Regex.
        /// </summary>
        public static string StripTagsRegexCompiled(string source)
        {
            return _htmlRegex.Replace(source, string.Empty);
        }

        /// <summary>
        /// Remove HTML tags from string using char array.
        /// </summary>
        public static string StripTagsCharArray(string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }

        public static string FiltraHtml(string text)
        {
            string txt = text.Replace("&ccedil;", "ç");
            txt = txt.Replace("&acirc;", "â").Replace("&ecirc;", "ê").Replace("&icirc;", "î").Replace("&ocirc;", "ô").Replace("&ucirc;", "û");
            txt = txt.Replace("&atilde;", "ã").Replace("&otilde;", "õ").Replace("&ntilde;", "ñ");
            txt = txt.Replace("&aacute;", "á").Replace("&eacute;", "é").Replace("&iacute;", "í").Replace("&uacute;", "ú").Replace("&oacute;", "ó");
            txt = txt.Replace("&Aacute;", "Á").Replace("&Eacute;", "É").Replace("&Iacute;", "Í").Replace("&Uacute;", "Ú").Replace("&Oacute;", "Ó");
            txt = txt.Replace("&agrave;", "à").Replace("&egrave;", "è").Replace("&ograve;", "ò").Replace("&ugrave;", "ù");
            txt = txt.Replace("&auml;", "ä").Replace("&euml;", "ë").Replace("&ouml;", "ï").Replace("&ouml;", "ö").Replace("&uuml;", "ü");
            txt = txt.Replace("&quot;", "\"").Replace("&ordf;", "ª").Replace("&nbsp;", "").Replace("&ordm;", "º").Replace("&ldquo;", "\"").Replace("&rdquo;", "\"");

            return txt;
        }
    }
}
