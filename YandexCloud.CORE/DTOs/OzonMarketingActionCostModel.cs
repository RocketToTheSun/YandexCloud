namespace YandexCloud.CORE.DTOs
{
    public class OzonMarketingActionCostModel
    {
        public int id { get; set; }
        public decimal amount { get; set; }
        public string operation_id { get; set; }
        public DateTime date { get; set; }
        public int clients_id { get; set; }

    }
}
