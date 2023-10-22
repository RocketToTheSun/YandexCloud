using YandexCloud.CORE.DTOs;

namespace YandexCloud.INIT
{
    public interface IRequestReader
    {
        IEnumerable<RequestDataDto> Read();
    }
}