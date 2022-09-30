using Camunda.WHttpClient.Connections;
using Camunda.WHttpClient.Entities.Users;
using Camunda.WHttpClient.Data.Contexts;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Nodes;

namespace Camunda.WHttpClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;   
        private Connection _connectionClient;
        private AppDbContext _context;
        public UserController(ILogger<UserController> logger, ILogger<Connection> conlogger, AppDbContext context)
        {
            _logger = logger;
            _connectionClient = new Connection(conlogger);
            _context = context;
        }


        //JSON Body olarak gönderilirse
        [HttpPost]
        [Route("CreateUserFromJSON")]
        public async Task<IActionResult> CreateUserFromJSON([FromBody] UserProfile userProfile)
        {
            try
            {
                _logger.LogInformation($"Sending request to create user with id {userProfile.profile.id}");
               
                
                //Log4Net tarafında custom bir değişkeni mesaja yazmak için aşağıdaki satır kullanılabilir.
                //ThreadContext.Properties["id"] = userProfile.profile.id;


                var result = _connectionClient.SendJSONRequest(userProfile, "user/create", "POST");

                _logger.LogInformation($"Request has been sent to create user with id {userProfile.profile.id}");


                _logger.LogInformation($"Trying to add user to database with id :  {userProfile.profile.id}");

                _context.Add(userProfile.profile);
                _context.SaveChanges();

                _logger.LogInformation($"User added to database with id :  {userProfile.profile.id}");

                return Ok(result);


            }
            catch (Exception ex)
            {
                _logger.LogError($"CreateUserException : An error occured while creating user with id {userProfile.profile.id}" + ex.Message);
                return BadRequest($"CreateUserException : An error occured while creating user with id {userProfile.profile.id}" + ex.Message);
            }

        }


        //JSON Body olarak gönderilirse
        [HttpPut]
        [Route("UpdateUser/{userId}/credentials")]
        public async Task<IActionResult> UpdateUser([FromBody] CredentialInformation credentials, string userId)
        {
            try
            {

                _logger.LogInformation($"Sending request to update user with id : {userId}");

                var result = _connectionClient.SendJSONRequest(credentials, $"user/{userId}/credentials", "PUT");

                _logger.LogInformation($"Request has been sent to update user with id : {userId}");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"CreateUserException : An error occured while creating user with id {userId}" + ex.Message);
                return BadRequest($"CreateUserException : An error occured while creating user with id {userId}" + ex.Message);
            }

        }


        [HttpDelete]
        [Route("DeleteUser/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                _logger.LogInformation($"Sending request to delete user with id : {userId}");

                var result = _connectionClient.SendHttpRequest($"user/{userId}", "DELETE");

                _logger.LogInformation($"Request has been sent to delete user with id : {userId}");

                var user = _context.Users.FirstOrDefault(x => x.id == userId);
                _context.Remove(user);
                _context.SaveChanges();

                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError($"DeleteUserException : An error occured while deleting user with id {userId}" + ex.Message);
                return BadRequest($"DeleteUserException : An error occured while deleting user with id {userId}" + ex.Message);
            }

        }


        [HttpGet]
        [Route("GetUserProfile/{userId}/profile")]
        public async Task<IActionResult> GetUserProfile(string userId)
        {
            try
            {
                _logger.LogInformation($"Sending request to get user profile with id : {userId}");

                var result = _connectionClient.SendHttpRequest($"user/{userId}/profile", "GET");


                _logger.LogInformation($"Request has been sent to get user profile with id : {userId}");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetUserProfileException : An error occured while getting user with id {userId}" + ex.Message);
                return BadRequest($"GetUserProfileException : An error occured while getting user with id {userId}" + ex.Message);
            }
         
        }

    }
}
