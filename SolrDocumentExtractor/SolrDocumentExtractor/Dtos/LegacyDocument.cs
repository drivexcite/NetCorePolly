using Newtonsoft.Json;

namespace SolrDocumentExtractor.Dtos
{
    public class LegacyDocument
    {
        [JsonProperty("Hwid")]
        public string Hwid { get; set; }

        [JsonProperty("Title_en_us_s")]
        public string Title { get; set; }

        [JsonProperty("Rank")]
        public string Rank { get; set; }
    }
}