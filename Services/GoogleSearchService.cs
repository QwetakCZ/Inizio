using InzioTest.Models;
using HtmlAgilityPack;
using System.Text;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Web;

namespace InzioTest.Services
{
    public class GoogleSearchService
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiKey = "AIzaSyAsxWF-DmWLzzMLYCJYTvw10z6v8Ct4pwo";
        private readonly string _searchEngineId = "d0a3f265e786844b9";

        public async Task<List<SearchResult>> SearchAPIAsync(string keyword)
        {
            using (var httpClient = new HttpClient())
            {
                var encodedKeyword = Uri.EscapeDataString(keyword);//Potreba prevest na URL format problem se specialnimy znaky asi hodit do metody
                var requestUrl = $"https://www.googleapis.com/customsearch/v1?q={encodedKeyword}&cx={_searchEngineId}&key={_apiKey}";
                var response = await httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                var json = JObject.Parse(content);
                var results = new List<SearchResult>();
                if (json["items"] == null)
                {
                    return null;
                }
                foreach (var item in json["items"])
                {
                    results.Add(new SearchResult
                    {
                        Title = item["title"].ToString(),
                        Url = item["link"].ToString()
                    });
                }

                return results;
            }
        }

        public async Task<List<SearchResult>> SearchXPathAsync(string keyword)
        {
            using (var httpClient = new HttpClient())
            {
                var encodedKeyword = Uri.EscapeDataString(keyword);
                var response = await httpClient.GetAsync($"https://www.google.com/search?q={encodedKeyword}");
                var content = await response.Content.ReadAsStringAsync();
                 
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(content);

                var resultNodes = htmlDocument.DocumentNode.SelectNodes("//div[contains(@class, 'BNeawe vvjwJb AP7Wnd')]/ancestor::a");
                
                if (resultNodes == null)
                {
                    return new List<SearchResult>();
                }

                var results = resultNodes.Select(node => new SearchResult
                {
                    Title = DecodeHtmlEntities(node.SelectSingleNode(".//div[contains(@class, 'BNeawe vvjwJb AP7Wnd')]").InnerText),
                    Url = RemoveUrlCharacters(HttpUtility.UrlDecode(node.GetAttributeValue("href", string.Empty).Substring(7)))
                }).ToList();

                return results;
            }
        }

        private string DecodeHtmlEntities(string input)
        {
            return WebUtility.HtmlDecode(input);
        }

        private string RemoveUrlCharacters(string input)
        {
            var index = input.IndexOf("&amp");
            return index == -1 ? input : input.Substring(0, index);
        }


    }
}
