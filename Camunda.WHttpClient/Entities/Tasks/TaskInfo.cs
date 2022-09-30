namespace Camunda.WHttpClient.Tasks
{
    public class TaskInfo
    {
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string name { get; set; }
        public string description { get; set; }
        public int priority { get; set; }
        public string assignee { get; set; }
        public string owner { get; set; }
        public string delegationState { get; set; }
        public string due { get; set; } = DateTime.Now.ToString();
        public string followUp { get; set; } = DateTime.Now.ToString();
        public string? parentTaskId { get; set; } = null;
        public string ?caseInstanceId { get; set; }
        public string ?tenantId { get; set; }
    }
}
