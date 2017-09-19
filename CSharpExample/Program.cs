using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace CSharpExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var html = GetHtmlContent("https://en.wikipedia.org/wiki/Comparison_of_programming_languages");
            IList<RowTableContent> table = GetTableContentFromHtml(html);

            var groupedData = table.GroupBy(x => x.Functional);

            foreach (var item in groupedData)
            {
                Console.WriteLine($"{item.Key} {item.Count()}");
            }
        }

        private static IList<RowTableContent> GetTableContentFromHtml(string htmlContent)
        {
            var result = new List<RowTableContent>();

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            var table = doc.DocumentNode.SelectNodes("//*[@id=\"mw-content-text\"]/div/table[3]")[0];
            var trs = table.ChildNodes.Where(x => x.Name == "tr");        
            var rows = trs.Skip(1).SkipLast(1);

            foreach (var r in rows)
            {
                var rtc = new RowTableContent();
                var functionalText = r.SelectNodes("td")[3].InnerText;
                rtc.Descricao = r.SelectNodes("th")[0].InnerText;
                rtc.Functional = new Regex("(Yes\\[.*\\]|Yes)").Replace(functionalText,"Yes");
                result.Add(rtc);
            }
            
            return result;
        }

        private static string GetHtmlContent(string uri)
        {
            var result = String.Empty;
                                    
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream receiveStream = response.GetResponseStream())
                {
                    using(StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                        result = readStream.ReadToEnd();
                        readStream.Close();
                    }
                    receiveStream.Close();
                }
                response.Close();
            }
            return result;
        }
    }
}
