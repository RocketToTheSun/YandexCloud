namespace YandexCloud.CORE.Models
{
    public class RequestDataModel
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string ApiKey { get; set; }
        public string ClientId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; }
        public string OperationType { get; set; }
    }
}
