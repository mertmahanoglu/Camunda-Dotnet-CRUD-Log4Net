using Camunda.WHttpClient.Connections;
using Camunda.WHttpClient.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Camunda.WHttpClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ILogger<TaskController> _logger;
        private readonly Connection _connectionClient;


        public TaskController(ILogger<TaskController> logger, ILogger<Connection> conlogger)
        {
            _logger = logger;
            _connectionClient = new Connection(conlogger);
        }


        [HttpGet("GetTasks")]
        public async Task<IActionResult> GetTasks()
        {
            try
            {
                _logger.LogInformation($"Sending request to getting all tasks");
                var result = _connectionClient.SendHttpRequest("task", "GET");
                _logger.LogInformation($"Request has been sent to getting all tasks");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("GetTasksException : An error occured while getting all tasks : " + ex.Message);
                return BadRequest("GetTasksException : An error occured while getting all tasks : " + ex.Message);
            }
        
        }



        [HttpPost("CreateTask")]
        public async Task<IActionResult> CreateTask([FromBody] TaskInfo taskInfo)
        {

            try
            {
                _logger.LogInformation($"Sending request to create task with info : {taskInfo}");
                var result = _connectionClient.SendJSONRequest(taskInfo, "task/create", "POST");

                _logger.LogInformation($"Request has been sent to create task with info {taskInfo}");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"CreateTaskException : An error occured while creating task with info {taskInfo}: " + ex.Message);
                return BadRequest($"CreateTaskException : An error occured while creating task with info {taskInfo}: " + ex.Message);
            }

        }



        [HttpDelete("DeleteTask/{taskId}")]
        public async Task<IActionResult> DeleteTask(string taskId)
        {

            try
            {

                _logger.LogInformation($"Sending request to delete task with id {taskId}");
                var result = _connectionClient.SendHttpRequest($"task/{taskId}", "DELETE");
                _logger.LogInformation($"Request to delete task with id {taskId} has been sent");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"DeleteTaskException : Error occured while deleting task with id {taskId} " + ex.Message);
                return BadRequest($"DeleteProcessException : Error occured while deleting task with id {taskId}" + ex.Message);
            }
        }



        [HttpPut("UpdateTask/{taskId}")]
        public async Task<IActionResult> UpdateTask([FromBody] TaskInfo taskInfo,string taskId)
        {
            try
            {
                _logger.LogInformation($"Sending request to update task. Task id : {taskId}");

                var result = _connectionClient.SendJSONRequest(taskInfo, $"task/{taskId}", "PUT");

                _logger.LogInformation($"Request has been sent to update task. Task id :  {taskId}");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"UpdateTaskException : Error occured while updating task: Task id :  {taskId} " + ex.Message);
                return BadRequest($"UpdateTaskException : Error occured while updating task: Task id :  {taskId} " + ex.Message);
            }
        }
    }
}
