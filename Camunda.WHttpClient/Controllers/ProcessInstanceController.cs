using Camunda.WHttpClient.Entities.Variables;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using log4net;
using Camunda.WHttpClient.Connections;

namespace Camunda.WHttpClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessInstanceController : ControllerBase
    {
        private readonly ILogger<ProcessInstanceController> _logger;
        private Connection _connectionClient;


        public ProcessInstanceController(ILogger<ProcessInstanceController> logger, ILogger<Connection> conlogger)
        {
            _logger = logger;
            _connectionClient = new Connection(conlogger);
        }

        [HttpPost("StartProcess/key/{processName}")]
        public async Task<IActionResult> StartProcess(string processName)
        {
            try
            {
                _logger.LogInformation($"Sending request to start {processName} process");
               
                var query = new Dictionary<string, string>()
                {
                    ["workerId"] = "dotnetWorker",
                    ["key"] = processName
                };
                var result = _connectionClient.SendJSONRequest(query,$"process-definition/key/{processName}/start", "POST");

                _logger.LogInformation($"Request has been sent to start {processName} process");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"StartProcessException : An error occured while starting  {processName} process: " + ex.Message);
                return BadRequest($"StartProcessException : An error occured while starting  {processName} process: " + ex.Message);
            }
        }



        [HttpDelete("DeleteProcess/{processId}")]
        public async Task<IActionResult> DeleteProcess(string processId)
        {
            try
            {
                _logger.LogInformation($"Sending request to delete process with id {processId}");
                var result = _connectionClient.SendHttpRequest($"process-instance/{processId}", "DELETE");
                _logger.LogInformation($"Request to delete process with id {processId} has been sent");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"DeleteProcessException : Error occured while deleting process with id {processId} " + ex.Message);
                return BadRequest($"DeleteProcessException : Error occured while deleting process with id {processId }" + ex.Message);
            }

        }

        [HttpGet("GetProcessVariables/{processId}")]
        public async Task<IActionResult> GetProcessVariables(string processId)
        {
            try
            {
                _logger.LogInformation($"Sending request to get process variables. Process id :  {processId}");

                var result = _connectionClient.SendHttpRequest($"process-instance/{processId}/variables", "GET");

                _logger.LogInformation($"Request has been sent to get process variables. Process id :  {processId}");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetProcessVariablesException : Error occured while getting variables: Process id :  {processId} " + ex.Message);
                return BadRequest($"GetProcessVariablesException : Error occured while getting variables: Process id :  {processId} " + ex.Message);
            }
        }


        [HttpPost("UpdateVariables/{processId}")]
        public async Task<IActionResult> UpdateVariables([FromBody] Variables variables,string processId)
        {

            try
            {
                _logger.LogInformation($"Sending request to update process variables. Process id :  {processId}");

                var result = _connectionClient.SendJSONRequest(variables,$"process-instance/{processId}/variables", "POST");

                _logger.LogInformation($"Request has been sent to update process variables. Process id :  {processId}");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"UpdateVariablesException : Error occured while updating variables: Process id :  {processId} " + ex.Message);
                return BadRequest($"UpdateVariablesException : Error occured while updating variables: Process id :  {processId} " + ex.Message);
            }
        }


        [HttpGet("GetProcessInstance")]
        public async Task<IActionResult> GetProcessInstance()
        {

            try
            {
                _logger.LogInformation($"Sending request to getting all processes");
                var result =  _connectionClient.SendHttpRequest("process-instance", "GET");
                _logger.LogInformation($"Request has been sent to getting all processes");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("GetProcessInstanceException : An error occured while getting all processes : " + ex.Message);
                return BadRequest("GetProcessInstanceException : An error occured while getting all processes : " + ex.Message);
            }
        }

 


    }
}
