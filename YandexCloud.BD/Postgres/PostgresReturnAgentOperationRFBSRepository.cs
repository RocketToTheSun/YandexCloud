using Dapper;
using Npgsql;
using YandexCloud.CORE.DTOs;

namespace YandexCloud.BD.Postgres
{
    public class PostgresReturnAgentOperationRFBSRepository : IOzonData<IEnumerable<ReturnAgentOperationRFBSModel>>
    {
        NpgsqlConnection _connection;

        public PostgresReturnAgentOperationRFBSRepository(NpgsqlConnection connection) => _connection = connection;

        public async Task CreateAsync(IEnumerable<ReturnAgentOperationRFBSModel> data)
        {
            var sql = "insert into return_agent_operation_rfbs (sku, name, amount, posting_number, operation_id, date, clients_id) " +
                "values (@sku, @name, @amount, @posting_number, @operation_id, @date, @clients_id)";

            await _connection.ExecuteAsync(sql, data);
        }

        public Task<IEnumerable<ReturnAgentOperationRFBSModel>> ReadAsync()
        {
            throw new NotImplementedException();
        }
    }
}
