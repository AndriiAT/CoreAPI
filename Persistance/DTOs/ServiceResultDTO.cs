namespace Persistance.DTOs
{
    public class ServiceResultDTO<T>
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }
        public T Data { get; set; }
    }
}
