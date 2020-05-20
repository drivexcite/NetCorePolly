using Newtonsoft.Json;

namespace SolrDocumentExtractor.Dtos
{
    public class LegacyDocument
    {
        [JsonProperty("hwid")]
        public string Hwid { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("rank")]
        public string Rank { get; set; }
    }
}