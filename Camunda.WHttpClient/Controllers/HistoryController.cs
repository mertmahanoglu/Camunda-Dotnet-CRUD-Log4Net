using Camunda.WHttpClient.Connections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Camunda.WHttpClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {

        private readonly ILogger<HistoryController> _logger;
        private Connection _connectionClient;


        public HistoryController(ILogger<HistoryController> logger, ILogger<Connection> conlogger)
        {
            _logger = logger;
            _connectionClient = new Connection(conlogger);
        }


        [HttpGet("GetHistory")]
        public async Task<IActionResult> GetHistory()
        {
            try
            {
                _logger.LogInformation("Sending HTTP request to get history");

                var result = _connectionClient.SendHttpRequest("history/activity-instance", "GET");
                var jsonResult = JsonConvert.SerializeObject(result);
                _logger.LogInformation("HTTP request sent to get history");
                return Ok(jsonResult);
            }
            catch (Exception ex)
            {
                _logger.LogError("GetHistoryException Error occured while calling GetHistory. Message : " + ex.Message);
                return BadRequest("GetHistoryException Error occured while calling GetHistory. Message : " + ex.Message);
            }
        }



        [HttpGet("GetUserHistory")]
        public async Task<IActionResult> GetUserHistory()
        {
            try
            {
                _logger.LogInformation("Sending HTTP request to get user history");

                var result = _connectionClient.SendHttpRequest("history/user-operation", "GET");
                var jsonResult = JsonConvert.SerializeObject(result);
                _logger.LogInformation("HTTP request sent to get history");
                return Ok(jsonResult);
            }
            catch (Exception ex)
            {
                _logger.LogError("GetUserHistoryException Error occured while calling GetHistory. Message : " + ex.Message);
                return BadRequest("GetUserHistoryException Error occured while calling GetHistory. Message : " + ex.Message);
            }
        }

    }
}
