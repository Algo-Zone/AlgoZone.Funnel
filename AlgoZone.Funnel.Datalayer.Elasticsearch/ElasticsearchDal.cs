using System;
using System.Threading.Tasks;
using Nest;

namespace AlgoZone.Funnel.Datalayer.Elasticsearch
{
    public class ElasticsearchDal
    {
        #region Fields

        private readonly IElasticClient _client;

        #endregion

        #region Constructors

        public ElasticsearchDal(string hostname, string port, string index)
        {
            var settings = new ConnectionSettings(new Uri($"http://{hostname}:{port}")).DefaultIndex(index);
            _client = new ElasticClient(settings);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a document to the elasticsearch database.
        /// </summary>
        /// <param name="document">The document to add.</param>
        /// <typeparam name="TDocumentType">The type of the document.</typeparam>
        public void AddDocument<TDocumentType>(TDocumentType document)
            where TDocumentType : class
        {
            _client.IndexDocument(document);
        }

        /// <summary>
        /// Adds a document to the elasticsearch database asynchronously.
        /// </summary>
        /// <param name="document">The document to add.</param>
        /// <typeparam name="TDocumentType">The type of the document.</typeparam>
        public async Task AddDocumentAsync<TDocumentType>(TDocumentType document)
            where TDocumentType : class
        {
            await _client.IndexDocumentAsync(document);
        }

        #endregion
    }
}