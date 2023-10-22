using Dapper;
using Npgsql;
using YandexCloud.BD;
using YandexCloud.CORE.DTOs;

namespace YandexCloud.CORE.Repositories
{
    public class PostgresOperationReturnGoodsFbsOfRmsRepository : IOzonData<IEnumerable<OperationReturnGoodsFBSofRMSModel>>
    {
        NpgsqlConnection _connection;

        public PostgresOperationReturnGoodsFbsOfRmsRepository(NpgsqlConnection connection) => _connection = connection;

        public async Task CreateAsync(IEnumerable<OperationReturnGoodsFBSofRMSModel> data)
        {
            var sql = "insert into operation_return_goods_fbsof_rms " +
                "values (@id, @amount, @operation_id, @date, @sku, @name, @posting_number, @clients_id)";

            await _connection.ExecuteAsync(sql, data);
        }

        public Task<IEnumerable<OperationReturnGoodsFBSofRMSModel>> ReadAsync()
        {
            throw new NotImplementedException();
        }
    }
}
