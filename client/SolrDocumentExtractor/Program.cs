using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using SolrDocumentExtractor.DataAccess;
using SolrDocumentExtractor.Dtos;
using SolrDocumentExtractor.Processing;

namespace SolrDocumentExtractor
{
    class Program
    {
        public static async Task<List<LegacyDocument>> ProcessSolrDocuments(int page, int pageSize)
        {
            Console.WriteLine($"Starting downloading documents from page {page + 1} on thread {Thread.CurrentThread.ManagedThreadId}.");

            var stopWatch = new Stopwatch();
            var start = page * pageSize;

            stopWatch.Start();
            var documents = await SolrGateway.GetDocumentsFromSolr(start, pageSize);

            SolrDocumentProcessor.ProcessDocuments(documents);

            stopWatch.Stop();
            Console.WriteLine($"Completed downloading documents from page {page + 1} on thread {Thread.CurrentThread.ManagedThreadId}. Elapsed: {stopWatch.Elapsed}");

            return documents;
        }

        static async Task Main()
        {
            var numberOfDocuments = await SolrGateway.GetAvailableDocumentsFromSolr();
            const int pageSize = 100;

            var pages = (int)Math.Ceiling(numberOfDocuments / (decimal)pageSize);

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var completedSuccessfully = 0;

            for (var page = 0; page < pages; page++)
            {
                var currentPage = page;
                await ProcessSolrDocuments(currentPage, pageSize);

                completedSuccessfully++;
            }

            Console.WriteLine($"Completed successfully: {completedSuccessfully}");

            Console.WriteLine($"Execution finished in: {stopWatch.Elapsed}");
            stopWatch.Stop();
        }
    }
}
