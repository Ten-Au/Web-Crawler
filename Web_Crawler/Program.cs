using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace Web_Crawler
{
    internal class Program
    {
        
        static async Task Main(string[] args)
        {
            string websiteUrl = args[0];
            if (websiteUrl == null)
            {
                throw new ArgumentNullException("Null value passed error");
            }
            if (!Uri.IsWellFormedUriString(websiteUrl, UriKind.Absolute))
            {
                throw new ArgumentException("Invalid URL error");
            }


                HttpClient httpClient = new ();
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(websiteUrl);
                if (response.IsSuccessStatusCode)
                {
                    string htmlContent = await response.Content.ReadAsStringAsync();
                    if (htmlContent == null)
                    {
                        throw new ArgumentNullException("Error while downloading the page");
                    }
                    var regex = new Regex("[a-z]+[a-z0-9-]*@[a-z0-9-]+\\.[a-z]+", RegexOptions.IgnoreCase);
                    
                    
                    var matches = regex.Matches(htmlContent);
                    
                    if (matches.Count == 0)
                    {
                        throw new ArgumentNullException("No e-mail addresses found.");
                    }

                    HashSet<string> matchesHashSet = new HashSet<string>();
                    foreach (Match match in matches)
                    {
                        matchesHashSet.Add(match.ToString());
                    }

                    foreach (String match in matchesHashSet)
                    {             
                        Console.WriteLine(match);
                        
                    }
                    httpClient.Dispose();
                }
                else
                {
                    throw new HttpRequestException();
                }
            }catch (HttpRequestException ex)
            {
                Console.WriteLine("The request failed");
            }
        }
    }
}
