using YandexCloud.CORE.DTOs;

namespace YandexCloud.CORE.BL.Managers
{
    public interface IOzonManager
    {
        event Action<string> OzonEventHandler;

        Task HandleOzonData(RequestDataDto requestDto);
    }
}