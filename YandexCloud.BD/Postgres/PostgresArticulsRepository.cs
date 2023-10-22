using Dapper;
using Npgsql;
using YandexCloud.CORE.DTOs.Articuls;

namespace YandexCloud.BD.Postgres
{
    public class PostgresArticulsRepository : IOzonData<IEnumerable<Item>>
    {
        readonly NpgsqlConnection _connection;

        public PostgresArticulsRepository(NpgsqlConnection connection) => _connection = connection;

        public async Task CreateAsync(IEnumerable<Item> model)
        {
            var sql = "insert into articuls values (@offer_id, @name, @fbo_sku, @fbs_sku)";
            await _connection.ExecuteAsync(sql, model);
        }

        public async Task<IEnumerable<Item>> ReadAsync()
        {
            var sql = "select * from articuls";
            return await _connection.QueryAsync<Item>(sql);
        }
    }
}
