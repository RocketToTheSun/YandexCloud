using Dapper;
using Npgsql;
using YandexCloud.CORE.DTOs;

namespace YandexCloud.BD.Postgres
{
    public class PostgresServiceNamesRepository : IServiceNamesRepository
    {
        NpgsqlConnection _connection;

        public PostgresServiceNamesRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<OzonServiceNamesDto>> GetAsync()
        {
            var sql = "select * from ozon_service_names";
            return await _connection.QueryAsync<OzonServiceNamesDto>(sql);
        }
    }
}
