using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TestProject.Models;

namespace TestProject.DataServices
{
    public class HttpDataClient : IDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<CovidSummary> GetSummaries()
        {
            var response = await _httpClient.GetAsync($"{_configuration["Covid19API"]}/summary/");

            if (response.IsSuccessStatusCode)
            {
                using var contentStream =
                    await response.Content.ReadAsStreamAsync();

                var results = await JsonSerializer.DeserializeAsync
                    <CovidSummary>(contentStream);
                return results;
            }
            else return null;
        }
        public async Task<IEnumerable<CovidHistory>> GetHistory(string country)
        {
            var response = await _httpClient.GetAsync($"{_configuration["Covid19API"]}/total/country/{country}");

            if (response.IsSuccessStatusCode)
            {
                using var contentStream =
                    await response.Content.ReadAsStreamAsync();

                var results = await JsonSerializer.DeserializeAsync
                    <IEnumerable<CovidHistory>>(contentStream);
                return results;
            }
            else return null;
        }

    }
}