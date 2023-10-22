using YandexCloud.CORE.DTOs;

namespace YandexCloud.BD
{
    public interface IOzonData<T> where T : class
    {
        Task CreateAsync(T model);
        Task<T> ReadAsync();
    }
}