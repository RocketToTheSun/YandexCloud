using Dapper;
using Npgsql;
using YandexCloud.CORE.DTOs;

namespace YandexCloud.BD.Postgres
{
    public class OzonSecondDataRepository : IOzonSecondDataRepository
    {
        NpgsqlConnection _connection;

        public OzonSecondDataRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public async Task CreateAsync(IEnumerable<OzonSecondDataDto> data)
        {
            var sql = "insert into second_table (first_table_id, price, ozon_service_names_id) " +
                        "values (@first_table_id, @price, @ozon_service_names_id)";
            await _connection.ExecuteAsync(sql, data);
        }
    }
}
