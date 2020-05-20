using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SolrDocumentExtractor.Dtos;
using SolrDocumentExtractor.Exceptions;

namespace SolrDocumentExtractor.DataAccess
{
    public class SolrGateway
    {
        public static HttpClient HttpClient { get; } = new HttpClient();

        public static async Task<List<LegacyDocument>> GetDocumentsFromSolr(int start, int rows)
        {
            var url = $"http://localhost:8080/documents?skip={start}&top={rows}";
            var response = await HttpClient.GetAsync(url);

            if(!response.IsSuccessStatusCode)
                throw new InvalidSolrResponseException($"The server responded with: {response.StatusCode} for {url}");

            var responseJson = JToken.Parse(await response.Content.ReadAsStringAsync());

            return (
                from document in responseJson["items"]
                select document.ToObject<LegacyDocument>()
            ).ToList();
        }

        public static async Task<int> GetAvailableDocumentsFromSolr()
        {
            var url = $"http://localhost:8080/documents?skip=0&top=0";
            var response = await HttpClient.GetAsync(url);
            var responseJson = JToken.Parse(await response.Content.ReadAsStringAsync());

            return responseJson["available"].Value<int>();
        }
    }
}