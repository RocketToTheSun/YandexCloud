using Dapper;
using Npgsql;
using YandexCloud.CORE.DTOs;

namespace YandexCloud.BD.Postgres
{
    public class PostgresMarketinActionsRepository : IOzonData<IEnumerable<OzonMarketingActionCostModel>>
    {
        NpgsqlConnection _connection;

        public PostgresMarketinActionsRepository(NpgsqlConnection connection) => _connection = connection;

        public async Task CreateAsync(IEnumerable<OzonMarketingActionCostModel> data)
        {
            var sql = "insert into marketing_actions (amount, operation_id, date, clients_id) " +
                "values (@amount, @operation_id, @date, @clients_id)";

            await _connection.ExecuteAsync(sql, data);
        }

        public Task<IEnumerable<OzonMarketingActionCostModel>> ReadAsync()
        {
            throw new NotImplementedException();
        }
    }
}
