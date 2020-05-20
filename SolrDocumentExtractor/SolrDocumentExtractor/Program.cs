using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using SolrDocumentExtractor.DataAccess;
using SolrDocumentExtractor.Processing;

namespace SolrDocumentExtractor
{
    class Program
    {
        public static async Task ProcessSolrDocuments(int page, int pageSize)
        {
            Console.WriteLine($"Starting downloading documents from page {page} on thread {Thread.CurrentThread.ManagedThreadId}.");

            var stopWatch = new Stopwatch();
            var start = page * pageSize;

            stopWatch.Start();
            var documents = await SolrGateway.GetDocumentsFromSolr(start, pageSize);

            SolrDocumentProcessor.ProcessDocuments(documents);

            stopWatch.Stop();
            Console.WriteLine($"Completed downloading documents from page {page} on thread {Thread.CurrentThread.ManagedThreadId}. Elapsed: {stopWatch.Elapsed}");
        }

        static async Task Main(string[] args)
        {
            var numberOfDocuments = await SolrGateway.GetAvailableDocumentsFromSolr();
            var pageSize = 100;

            var pages = Math.Ceiling(numberOfDocuments / (decimal)pageSize);

            var tasks = new List<Task>();

            for (var page = 0; page < pages; page++)
            {
                tasks.Add(ProcessSolrDocuments(page, pageSize));
            }

            await Task.WhenAll(tasks);
        }
    }
}
