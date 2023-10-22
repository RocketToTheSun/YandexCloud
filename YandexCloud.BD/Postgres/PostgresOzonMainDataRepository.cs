using Dapper;
using Npgsql;
using YandexCloud.CORE.DTOs;

namespace YandexCloud.BD.Postgres
{
    public class PostgresOzonMainDataRepository : IOzonData<IEnumerable<OzonFirstTableModel>>
    {
        NpgsqlConnection _connection;

        public PostgresOzonMainDataRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public async Task CreateAsync(IEnumerable<OzonFirstTableModel> data)
        {
            var sql = "insert into first_table " +
                "values (@id, @date, @sku, @name, @posting_number, @accruals_for_sale, @sale_comission, @clients_id)";
            await _connection.ExecuteAsync(sql, data);
        }

        public Task<IEnumerable<OzonFirstTableModel>> ReadAsync()
        {
            throw new NotImplementedException();
        }
    }
}
