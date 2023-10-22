using YandexCloud.CORE.DTOs;

namespace YandexCloud.BD.Postgres
{
    public interface IServiceNamesRepository
    {
        Task<IEnumerable<OzonServiceNamesDto>> GetAsync();
    }
}