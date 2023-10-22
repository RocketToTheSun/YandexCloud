namespace YandexCloud.CORE.DTOs
{
    public class PriceByOperationItemReturnModel
    {
        public int id { get; set; }
        public decimal price { get; set; }
        public int ozon_service_name_id { get; set; }
        public string operation_item_return_id { get; set; }
    }
}
