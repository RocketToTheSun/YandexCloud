namespace YandexCloud.BD.Models
{
    public class Date
    {
        public DateTime from { get; set; }
        public DateTime to { get; set; }
    }

    public class Filter
    {
        public Date date { get; set; }
        public List<string> operation_type { get; set; }
        public string posting_number { get; set; }
        public string transaction_type { get; set; }
    }

    public class TransactionListModel
    {
        public Filter filter { get; set; }
        public int page { get; set; }
        public int page_size { get; set; }
    }

}

