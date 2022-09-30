using Camunda.WHttpClient.Connections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Text;

namespace Camunda.WHttpClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalTaskController : ControllerBase
    {


        private readonly ILogger<ExternalTaskController> _logger;
        private Connection _connectionClient;

        public ExternalTaskController(ILogger<ExternalTaskController> logger, ILogger<Connection> conlogger)
        {
            _logger = logger;
            _connectionClient = new Connection(conlogger);
        }





        [HttpPost("CompleteExternalTask/{taskId}/complete")]
        public async Task<IActionResult> CompleteExternalTask(string taskId)
        {

            try
            {
                _logger.LogInformation($"Sending request to complete external task with id :{taskId}");
                var query = new Dictionary<string, string>()
                {
                    ["workerId"] = "someWorkerId",
                    ["key"] = taskId
                };
                var result = _connectionClient.SendJSONRequest(query, $"external-task/{taskId}/complete", "POST");
                _logger.LogInformation($"Task complete method has been completed with result : " + result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while completing external task with id :{taskId} \n " + ex.Message);
                return BadRequest($"An error occured while completing external task with id :{taskId} \n " + ex.Message);
            }

        }

        [HttpPost("Lock")]
        public async Task<IActionResult> Lock(string taskId, string lockDuration)
        {
            var query = new Dictionary<string, string>()
            {
                ["workerId"] = "someWorkerId",
                ["lockDuration"] = lockDuration
            };
            try
            {
                _logger.LogInformation($"Trying to lock external task with id :{taskId}");
                //var uri = QueryHelpers.AddQueryString(sb.ToString(), query);
                var result = _connectionClient.SendJSONRequest(query, $"external-task/{taskId}/lock", "POST");
                _logger.LogInformation($"Lock method has been completed with result : " + result);
                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while completing external task with id :{taskId} \n " + ex.Message);
                return BadRequest($"An error occured while completing external task with id :{taskId} \n " + ex.Message);
            }

        }
    }
}
