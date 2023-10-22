namespace YandexCloud.CORE.DTOs
{
    public class OzonAcquiringDataDto : OzonMarketingActionCostModel
    {
        public string sku { get; set; }
        public string name { get; set; }
        public string posting_number { get; set; }
        public int clients_id { get; set; }
    }
}
