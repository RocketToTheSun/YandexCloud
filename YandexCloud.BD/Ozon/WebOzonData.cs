using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using YandexCloud.BD.Models;
using YandexCloud.CORE.DTOs;
using YandexCloud.CORE.DTOs.Articuls;
using YandexCloud.CORE.Models;
using YandexCloud.CORE.Repositories;

namespace YandexCloud.BD.Ozon
{
    public class WebOzonData : IOzonFullData
    {
        readonly IHttpClientFactory _httpClientFactory;

        public WebOzonData(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<OzonResponseModel> GetDeliveryDataAsync(RequestDataModel requestModel)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var url = "https://api-seller.ozon.ru/v3/finance/transaction/list";

            var date = new Date
            {
                from = requestModel.From,
                to = requestModel.To,
            };

            var filter = new Filter
            {
                date = date,
                operation_type = new List<string> { requestModel.OperationType },
                posting_number = "",
                transaction_type = "all"
            };

            var transactionListModel = new TransactionListModel
            {
                filter = filter,
                page = requestModel.Page,
                page_size = 1000,
            };

            httpClient.DefaultRequestHeaders.Add("Client-Id", requestModel.ClientId);
            httpClient.DefaultRequestHeaders.Add("Api-Key", requestModel.ApiKey);

            using var content = new StringContent(JsonSerializer.Serialize(transactionListModel), Encoding.UTF8, "application/json");
            var responce = await httpClient.PostAsync(url, content);

            if (!responce.IsSuccessStatusCode)
                throw new HttpRequestException($"Получен код {responce.StatusCode}");

            var ozonResponceModel = await responce.Content.ReadFromJsonAsync<OzonResponseModel>();

            return ozonResponceModel;
        }

        public async Task<OzonArticulesResponseModel> GetArticulsAsync(RequestArticulsDto requestModel)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var url = "https://api-seller.ozon.ru/v2/product/info/list";

            httpClient.DefaultRequestHeaders.Add("Client-Id", requestModel.ClientId);
            httpClient.DefaultRequestHeaders.Add("Api-Key", requestModel.ApiKey);

            var request = new OzonArticulsRequestModel { sku = requestModel.sku };

            using var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var responce = await httpClient.PostAsync(url, content);

            if (!responce.IsSuccessStatusCode)
                throw new HttpRequestException($"Получен код {responce.StatusCode}");

            var temp = await responce.Content.ReadAsStringAsync();
            var ozonResponceModel = await responce.Content.ReadFromJsonAsync<OzonArticulesResponseModel>();

            return ozonResponceModel;
        }
    }
}
