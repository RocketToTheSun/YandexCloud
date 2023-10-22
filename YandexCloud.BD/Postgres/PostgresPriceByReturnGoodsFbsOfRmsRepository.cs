using Dapper;
using Npgsql;
using YandexCloud.CORE.DTOs;

namespace YandexCloud.BD.Postgres
{
    public class PostgresPriceByReturnGoodsFbsOfRmsRepository : IOzonData<IEnumerable<PriceByReturnGoodsFBSOfRMSModel>>
    {
        NpgsqlConnection _connection;

        public PostgresPriceByReturnGoodsFbsOfRmsRepository(NpgsqlConnection connection) => _connection = connection;

        public async Task CreateAsync(IEnumerable<PriceByReturnGoodsFBSOfRMSModel> data)
        {
            var sql = "insert into price_by_return_goods_fbs_of_rms (price, ozon_service_name_id, operation_return_goods_fbsof_rms_id) " +
                "values (@price, @ozon_service_name_id, @operation_return_goods_fbsof_rms_id)";

            await _connection.ExecuteAsync(sql, data);
        }

        public Task<IEnumerable<PriceByReturnGoodsFBSOfRMSModel>> ReadAsync()
        {
            throw new NotImplementedException();
        }
    }
}
