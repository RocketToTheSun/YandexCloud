using YandexCloud.CORE.DTOs;

namespace YandexCloud.CORE.Services
{
    public interface IBlService
    {
        event Action<string> OzonEventHandler;

        Task GetDataAsync(IEnumerable<RequestDataDto> requestModel);
    }
}