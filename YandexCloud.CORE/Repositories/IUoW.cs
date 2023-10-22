using YandexCloud.BD;
using YandexCloud.BD.Postgres;
using YandexCloud.CORE.DTOs;

namespace YandexCloud.CORE.Repositories
{
    public interface IUoW : IDisposable
    {
        IOzonData<IEnumerable<OzonFirstTableModel>> OzonMainDataRepository { get; }
        IOzonSecondDataRepository OzonSecondDataRepository { get; }
        IServiceNamesRepository OzonServiceNamesRepository { get; }
        IOzonData<IEnumerable<OzonAcquiringDataDto>> OzonAcquiringRepository { get; }
        IOzonData<IEnumerable<OzonMarketingActionCostModel>> OzonMarketingActionsRepository { get; }
        IOzonData<IEnumerable<ClientReturnAgentOperationModel>> OzonClientReturnAgentRepository { get; }
        IOzonData<IEnumerable<ReturnAgentOperationRFBSModel>> OzonReturnAgentOperationRFBSRepository { get; }
        IOzonData<IEnumerable<OperationReturnGoodsFBSofRMSModel>> OzonOperationReturnGoodsFbsOfRmsRepository { get; }
        IOzonData<IEnumerable<PriceByReturnGoodsFBSOfRMSModel>> OzonPriceByReturnGoodsFbsOfRmsRepository { get; }
        IOzonData<IEnumerable<OperationItemReturnModel>> OzonOperationItemReturnRepository { get; }
        IOzonData<IEnumerable<PriceByOperationItemReturnModel>> OzonPriceByOperationItemReturnRepository { get; }
        IOzonData<IEnumerable<PremiumCashbackIndividualPointsModel>> OzonPremiumCashbackIndividualPointsRepository { get; }
        IOzonData<IEnumerable<HoldingForUndeliverableGoodsModel>> OzonHoldingForUndeliverableGoodsRepository { get; }
        IOzonData<IEnumerable<OzonClientModel>> OzonClientRepository { get; }
        IOzonData<IEnumerable<DTOs.Articuls.Item>> OzonArticuslData { get; }

        Task CommitAsync();
        Task OpenTransactionAsync();
        Task RollbackAsync();
    }
}
