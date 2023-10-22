namespace YandexCloud.CORE.DTOs.Articuls
{
    public class OzonArticulesResponseModel
    {
        public Result result { get; set; }
    }

    public class Item
    {
        public string name { get; set; }
        public string offer_id { get; set; }
        public int fbo_sku { get; set; }
        public int fbs_sku { get; set; }
    }

    public class Result
    {
        public List<Item> items { get; set; }
    }
   
}
