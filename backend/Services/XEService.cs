using RateAlerts.Api.Interfaces;
using RateAlerts.Api.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static RateAlerts.Api.Interfaces.IRateXEService;

namespace RateAlerts.Api.Services
{

    public class XEService : IXeRateService
    {
        private readonly IConfiguration _configuration;

        public XEService(IConfiguration configuration) {
            _configuration = configuration;

        }
        public async Task<XERateResponse> GetRateAsync(string baseCurrency, string targetCurrency, CancellationToken cancellationToken)
        {
            //var results = new List<object>();

            // USD/CAD
            var client = new HttpClient();
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(
                _configuration["Xecd:AccountId"] + ":" + _configuration["Xecd:ApiKey"]));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
            string URL = $"https://xecdapi.xe.com/v1/convert_from.json/?from={baseCurrency}&to={targetCurrency}";
            var response = await client.GetAsync(URL);
            var body = response.Content.ReadAsStringAsync().Result;
            var doc = JsonDocument.Parse(body);
            var mid = doc.RootElement.GetProperty("to")[0].GetProperty("mid").GetDecimal();
            var timestamp = doc.RootElement.GetProperty("timestamp").GetString();
             var result= new XERateResponse (){ Pair = "USD/CAD", Rates = Math.Round(mid, 4), TimeStamp = timestamp };

            //// GBP/USD
            //var client2 = new HttpClient();
            //var credentials2 = Convert.ToBase64String(Encoding.ASCII.GetBytes(
            //    _configuration["Xecd:AccountId"] + ":" + _configuration["Xecd:ApiKey"]));
            //client2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials2);
            //var response2 = client2.GetAsync("https://xecdapi.xe.com/v1/convert_from.json/?from=GBP&to=USD").Result;
            //var body2 = response2.Content.ReadAsStringAsync().Result;
            //var doc2 = JsonDocument.Parse(body2);
            //var mid2 = doc2.RootElement.GetProperty("to")[0].GetProperty("mid").GetDecimal();
            //var timestamp2 = doc2.RootElement.GetProperty("timestamp").GetString();
            //results.Add(new { pair = "GBP/USD", rate = Math.Round(mid2, 4), asOf = timestamp2 });

            //// EUR/USD
            //var client3 = new HttpClient();
            //var credentials3 = Convert.ToBase64String(Encoding.ASCII.GetBytes(
            //    _configuration["Xecd:AccountId"] + ":" + _configuration["Xecd:ApiKey"]));
            //client3.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials3);
            //var response3 = client3.GetAsync("https://xecdapi.xe.com/v1/convert_from.json/?from=EUR&to=USD").Result;
            //var body3 = response3.Content.ReadAsStringAsync().Result;
            //var doc3 = JsonDocument.Parse(body3);
            //var mid3 = doc3.RootElement.GetProperty("to")[0].GetProperty("mid").GetDecimal();
            //var timestamp3 = doc3.RootElement.GetProperty("timestamp").GetString();
            //results.Add(new { pair = "EUR/USD", rate = Math.Round(mid3, 4), asOf = timestamp3 });

            return result;
        }
    }
}
