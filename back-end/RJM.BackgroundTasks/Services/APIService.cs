using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RJM.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RJM.BackgroundTasks.Services
{
    public class APIService
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<APIService> logger;
        private readonly IHttpClientFactory httpClientFactory;

        public APIService(
            IConfiguration configuration,
            ILogger<APIService> logger,
            IHttpClientFactory httpClientFactory
        )
        {
            this.configuration = configuration;
            this.logger = logger;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<HttpResponseMessage> HealthCheck()
        {
            HttpClient httpClient = this.httpClientFactory.CreateClient("RJM.API");

            // GET: api
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api");

            HttpResponseMessage response = await httpClient.SendAsync(request);

            return response;
        }

        public async Task<HttpResponseMessage> DocumentsCreateDocumentContents(Guid documentId, List<DocumentContentVM> documentContentVMs)
        {
            HttpClient httpClient = this.httpClientFactory.CreateClient("RJM.API");

            // POST: api/documents/{id}/contents
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Post,
                $"api/documents/{documentId}/contents"
            );

            // Request body
            string json = JsonConvert.SerializeObject(documentContentVMs);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.SendAsync(request);

            return response;
        }
    }
}
