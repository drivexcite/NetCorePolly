using System;
using System.Collections.Generic;
using System.Text;

namespace SolrDocumentExtractor.Exceptions
{
    public class InvalidSolrResponseException : Exception
    {
        public InvalidSolrResponseException(string message): base (message)
        {
            
        }
    }
}
