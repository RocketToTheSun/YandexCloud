﻿namespace YandexCloud.CORE.DTOs
{
    public class OzonResponseModel
    {
        public Result result { get; set; }
    }

    public class Result
    {
        public List<Operation> operations { get; set; }
        public int page_count { get; set; }
        public int row_count { get; set; }
    }

    public class Operation
    {
        public object operation_id { get; set; }
        public string operation_type { get; set; }
        public string operation_date { get; set; }
        public string operation_type_name { get; set; }
        public int delivery_charge { get; set; }
        public int return_delivery_charge { get; set; }
        public decimal? accruals_for_sale { get; set; }
        public decimal sale_commission { get; set; }
        public decimal amount { get; set; }
        public string type { get; set; }
        public Posting posting { get; set; }
        public List<Item> items { get; set; }
        public List<Service> services { get; set; }
    }

    public class Posting
    {
        public string delivery_schema { get; set; }
        public string order_date { get; set; }
        public string posting_number { get; set; }
        public object warehouse_id { get; set; }
    }

    public class Item
    {
        public string name { get; set; }
        public int sku { get; set; }
    }

    public class Service
    {
        public string name { get; set; }
        public decimal price { get; set; }
    }


}
