using System;
using System.Collections.Generic;
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
                var json = JsonConvert.SerializeObject(legacyDocument);
                //Console.WriteLine(json);
            }
        }
    }
}