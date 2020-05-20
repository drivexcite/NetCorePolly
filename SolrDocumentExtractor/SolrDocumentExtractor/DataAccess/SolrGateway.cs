using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SolrDocumentExtractor.Dtos;

namespace SolrDocumentExtractor.DataAccess
{
    public class SolrGateway
    {
        public static HttpClient HttpClient { get; } = new HttpClient();

        private static LegacyDocument ConvertToDocument(JToken json)
        {
            return new LegacyDocument
            {
                Hwid = json["Hwid"]?.Value<string>(),
                Title = json["Title_en_us_s"]?.Value<string>(),
                Rank = json["Rank"]?.Value<string>()
            };
        }

        public static async Task<List<LegacyDocument>> GetDocumentsFromSolr(int start, int rows)
        {
            var queryString = $"http://solr.test.hwapps.net:8983/solr/mcs.version.12.6/select?indent=on&q={{!term%20f=Taxon}}document&start={start}&rows={rows}&wt=json";
            var response = await HttpClient.GetAsync(queryString);
            var responseJson = JToken.Parse(await response.Content.ReadAsStringAsync());

            return (
                from document in responseJson["response"]["docs"]
                select ConvertToDocument(document)
            ).ToList();
        }

        public static async Task<int> GetAvailableDocumentsFromSolr()
        {
            var queryString = $"http://solr.test.hwapps.net:8983/solr/mcs.version.12.6/select?indent=on&q={{!term%20f=Taxon}}document&start=0&rows=0&wt=json";
            var response = await HttpClient.GetAsync(queryString);
            var responseJson = JToken.Parse(await response.Content.ReadAsStringAsync());

            return responseJson["response"]["numFound"].Value<int>();
        }
    }
}