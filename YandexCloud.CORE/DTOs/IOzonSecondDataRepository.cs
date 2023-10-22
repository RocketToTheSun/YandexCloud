using YandexCloud.CORE.DTOs;

namespace YandexCloud.BD.Postgres
{
    public interface IOzonSecondDataRepository
    {
        Task CreateAsync(IEnumerable<OzonSecondDataDto> data);
    }
}