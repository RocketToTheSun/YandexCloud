using Dapper;
using Npgsql;
using YandexCloud.CORE.DTOs;

namespace YandexCloud.BD.Postgres
{
    public class PostgresOperationItemReturnRepository : IOzonData<IEnumerable<OperationItemReturnModel>>
    {
        NpgsqlConnection _connection;

        public PostgresOperationItemReturnRepository(NpgsqlConnection connection) => _connection = connection;

        public async Task CreateAsync(IEnumerable<OperationItemReturnModel> data)
        {
            var sql = "insert into operation_item_return " +
                "values (@id, @amount, @operation_id, @date, @sku, @name, @posting_number, @clients_id)";

            await _connection.ExecuteAsync(sql, data);
        }

        public Task<IEnumerable<OperationItemReturnModel>> ReadAsync()
        {
            throw new NotImplementedException();
        }
    }
}
