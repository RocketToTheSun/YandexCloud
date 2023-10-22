using Dapper;
using Npgsql;
using YandexCloud.CORE.DTOs;

namespace YandexCloud.BD.Postgres
{
    public class PostgresHoldingForUndeliverableGoodsRepository : IOzonData<IEnumerable<HoldingForUndeliverableGoodsModel>>
    {
        NpgsqlConnection _connection;

        public PostgresHoldingForUndeliverableGoodsRepository(NpgsqlConnection connection) => _connection = connection;

        public async Task CreateAsync(IEnumerable<HoldingForUndeliverableGoodsModel> data)
        {
            var sql = "insert into marketplace_with_holding_for_undeliverable_goods (sku, name, amount, posting_number, operation_id, date, clients_id) " +
                "values (@sku, @name, @amount, @posting_number, @operation_id, @date, @clients_id)";

            await _connection.ExecuteAsync(sql, data);
        }

        public Task<IEnumerable<HoldingForUndeliverableGoodsModel>> ReadAsync()
        {
            throw new NotImplementedException();
        }
    }
}
