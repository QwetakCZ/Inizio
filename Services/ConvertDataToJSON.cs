using InzioTest.Models;
using Newtonsoft.Json;
namespace InzioTest.Services
{
    public class ConvertDataToJSON
    {
        //Metoda pro prevod Listu na JSON
        public string ConvertModelToJsonAsync(List<SearchResult> results)
        {
            var json = JsonConvert.SerializeObject(results);
            return json;
        }
    }
}
