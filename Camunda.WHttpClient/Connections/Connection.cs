using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using Swashbuckle.Swagger.Annotations;
using System.Net;
using System.Text;

namespace Camunda.WHttpClient.Connections
{
    public class Connection
    {
        private HttpClient _httpClient;
        private readonly ILogger<Connection> _logger;
        public Connection(ILogger<Connection> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:8080/engine-rest/");
        }

      
        public async Task<string> SendJSONRequest(object jsonObject, string apiURL,string requestType)
        {
            try
            {
                _logger.LogInformation("Sending HTTP request with JSON Body");
                string strUser = JsonConvert.SerializeObject(jsonObject);
                HttpContent message = new StringContent(strUser, Encoding.UTF8, "application/json");
                StringBuilder sb = new StringBuilder();
                sb.Append(_httpClient.BaseAddress.ToString());
                sb.Append(apiURL);

                _logger.LogInformation($"Sending {requestType} HTTP request to : " + sb.ToString());

                Task<HttpResponseMessage> request;

                switch (requestType)
                {
                    case "POST":
                        request = _httpClient.PostAsync(sb.ToString(), message);
                        break;
                    case "PUT":
                        request = _httpClient.PutAsync(sb.ToString(), message);
                        break;
                    default:
                        request = _httpClient.PostAsync(sb.ToString(), message);
                        break;
                }
                var response = await request.Result.Content.ReadAsStringAsync();
                _logger.LogInformation($"{requestType} has been sent to : " + sb.ToString());
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"SendJSONRequestException : Error occured while sending {requestType} HTTP request : " + ex.Message);
                throw new Exception($"SendJSONRequestException : Error occured while sending {requestType} HTTP request : " + ex.Message);
            }
          
        }



     
        public async Task<string> SendHttpRequest(string apiURL, string requestType)
        {
            try
            {
                _logger.LogInformation("Sending HTTP request");

                StringBuilder sb = new StringBuilder();
                sb.Append(_httpClient.BaseAddress.ToString());
                sb.Append(apiURL);
                Task<HttpResponseMessage> request;
                _logger.LogInformation($"Sending {requestType} HTTP request to : " + sb.ToString());
                switch (requestType)
                {
                    case "DELETE":
                        request = _httpClient.DeleteAsync(sb.ToString());
                        break;
                    case "GET":
                        request = _httpClient.GetAsync(sb.ToString());
                        break;
                    default:
                        request = _httpClient.GetAsync(sb.ToString());
                        break;
                }
                var response = await request.Result.Content.ReadAsStringAsync();
                _logger.LogInformation($"{requestType} has been sent to : " + sb.ToString());
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"SendJSONRequestException : Error occured while sending {requestType} HTTP request : " + ex.Message);
                throw new Exception($"SendJSONRequestException : Error occured while sending {requestType} HTTP request : " + ex.Message);
            }
           
        }

    }
}
