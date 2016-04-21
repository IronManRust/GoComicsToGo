using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GoComicsToGo
{

    internal static class Processor
    {

        internal static void Process(string name, DateTime min, DateTime max)
        {
            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), name));
            ConcurrentBag<DateTime> dates = new ConcurrentBag<DateTime>(Enumerable.Range(0, 1 + max.Subtract(min).Days)
                                                                                  .Select(x => min.AddDays(x)));
            Parallel.ForEach(dates, x => ProcessItem(name, x));
        }

        private static void ProcessItem(string name, DateTime date)
        {
            try
            {
                string webpageURL = GetWebpageURL(name, date);
                string webpageHTML = GetWebpageHTML(webpageURL);
                string stripURL = GetStripURL(webpageHTML);
                SaveStripSuccess(stripURL, name, date);
                Messenger.ProcessedItem(date, true);
            }
            catch
            {
                SaveStripFailure(name, date);
                Messenger.ProcessedItem(date, false);
            }
        }

        private static string GetWebpageURL(string name, DateTime date)
        {
            return string.Format("http://www.gocomics.com/{0}/{3}/{2}/{1}",
                                 name,
                                 date.Day.ToString().PadLeft(2, '0'),
                                 date.Month.ToString().PadLeft(2, '0'),
                                 date.Year.ToString().PadLeft(4, '0'));
        }

        private static string GetWebpageHTML(string webpageURL)
        {
            using (WebClient webClient = new WebClient())
            {
                return webClient.DownloadString(webpageURL);
            }
        }

        private static string GetStripURL(string webpageHTML)
        {
            Regex regex = new Regex(@"<img alt=""[\w\s]+"" class=""strip"" src=""http:\/\/assets\.amuniversal\.com\/([a-f0-9]{32})"" width=""600"" \/>");
            Match match = regex.Match(webpageHTML);
            if (match.Groups.Count == 2 && match.Groups[0].Success && match.Groups[1].Success)
            {
                return string.Format("http://assets.amuniversal.com/{0}", match.Groups[1].Value);
            }
            else
            {
                return null;
            }
        }

        private static void SaveStripSuccess(string stripURL, string name, DateTime date)
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile(stripURL, Path.Combine(Directory.GetCurrentDirectory(), name, string.Concat(date.ToString(Constants.DATE_MASK), ".gif")));
            }
        }

        private static void SaveStripFailure(string name, DateTime date)
        {
            File.Create(Path.Combine(Directory.GetCurrentDirectory(), name, string.Concat(date.ToString(Constants.DATE_MASK), ".error")));
        }

    }

}