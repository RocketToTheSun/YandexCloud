using Dapper;
using Npgsql;
using YandexCloud.CORE.DTOs;

namespace YandexCloud.BD.Postgres
{
    public class PostgresPriceByOperationItemReturnRepository : IOzonData<IEnumerable<PriceByOperationItemReturnModel>>
    {
        NpgsqlConnection _connection;

        public PostgresPriceByOperationItemReturnRepository(NpgsqlConnection connection) => _connection = connection;

        public async Task CreateAsync(IEnumerable<PriceByOperationItemReturnModel> data)
        {
            var sql = "insert into price_by_operation_item_return (price, ozon_service_name_id, operation_item_return_id) " +
                "values (@price, @ozon_service_name_id, @operation_item_return_id)";
         
            await _connection.ExecuteAsync(sql, data);
        }

        public Task<IEnumerable<PriceByOperationItemReturnModel>> ReadAsync()
        {
            throw new NotImplementedException();
        }
    }
}
