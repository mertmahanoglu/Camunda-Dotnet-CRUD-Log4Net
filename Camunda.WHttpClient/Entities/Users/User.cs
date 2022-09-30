using System.Text.Json.Nodes;

namespace Camunda.WHttpClient.Entities.Users
{
    public class User
    {
        public string id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
    }
}
