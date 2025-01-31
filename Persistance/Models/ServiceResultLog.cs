namespace Persistance.Models
{
    internal class ServiceResultLog
    {
        public string LogId { get; set; }

        public string MethodName { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
