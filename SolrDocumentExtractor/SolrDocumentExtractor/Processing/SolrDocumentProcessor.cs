using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using SolrDocumentExtractor.Dtos;

namespace SolrDocumentExtractor.Processing
{
    public class SolrDocumentProcessor
    {
        public static void ProcessDocuments(List<LegacyDocument> documents)
        {
            foreach (var legacyDocument in documents)
            {
                File.AppendAllText("LegacyDocuments.txt", $"{JsonConvert.SerializeObject(legacyDocument)}\n");
            }
        }
    }
}