using Dapper;
using Npgsql;
using YandexCloud.CORE.DTOs;

namespace YandexCloud.BD.Postgres
{
    public class PostgresOzonClientsRepository : IOzonData<IEnumerable<OzonClientModel>>
    {
        NpgsqlConnection _connection;

        public PostgresOzonClientsRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public async Task CreateAsync(IEnumerable<OzonClientModel> data)
        {
            var sql = "insert into clients " +
                "values (@id, @api_key)";
            await _connection.ExecuteAsync(sql, data);
        }

        public async Task<IEnumerable<OzonClientModel>> ReadAsync()
        {
            var sql = "select * from clients";
            return await _connection.QueryAsync<OzonClientModel>(sql);
        }
    }
}
