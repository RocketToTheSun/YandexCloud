using YandexCloud.CORE.DTOs;
using YandexCloud.CORE.DTOs.Articuls;
using YandexCloud.CORE.Models;

namespace YandexCloud.CORE.Repositories
{
    public interface IOzonFullData
    {
        Task<OzonArticulesResponseModel> GetArticulsAsync(RequestArticulsDto requestModel);
        Task<OzonResponseModel> GetDeliveryDataAsync(RequestDataModel requestModel);
    }
}
