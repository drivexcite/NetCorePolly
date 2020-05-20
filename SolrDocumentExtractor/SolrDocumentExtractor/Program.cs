using System;
using System.Threading.Tasks;
using SolrDocumentExtractor.DataAccess;
using SolrDocumentExtractor.Processing;

namespace SolrDocumentExtractor
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var numberOfDocuments = await SolrGateway.GetAvailableDocumentsFromSolr();
            var pageSize = 100;

            var pages = Math.Ceiling(numberOfDocuments / (decimal) pageSize);

            for (var page = 0; page < pages; page++)
            {
                var start = page * pageSize;
                var documents = await SolrGateway.GetDocumentsFromSolr(start, pageSize);

                SolrDocumentProcessor.ProcessDocuments(documents);
            }
        }
    }
}
