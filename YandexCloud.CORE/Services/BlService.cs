using YandexCloud.CORE.BL.Managers;
using YandexCloud.CORE.DTOs;
using YandexCloud.CORE.Repositories;

namespace YandexCloud.CORE.Services
{
    public class BlService : IBlService
    {
        readonly IOzonFullData _ozonFullData;
        readonly IUoW _uoW;
        public event Action<string> OzonEventHandler;

        public BlService(IOzonFullData ozon, IUoW uoW)
        {
            _ozonFullData = ozon;
            _uoW = uoW;
        }

        public async Task GetDataAsync(IEnumerable<RequestDataDto> requestDto)
        {
            var managerList = new List<OzonManager>();

            try
            {
                foreach (var dto in requestDto)
                {
                    var manager = new OzonManager(_ozonFullData, _uoW);
                    var clientModel = new OzonClientModel { id = dto.ClientId, api_key = dto.ApiKey };

                    var clientModelFromDb = await _uoW.OzonClientRepository.ReadAsync();
                    if (!clientModelFromDb.Any(c => c.id == clientModel.id))
                        await _uoW.OzonClientRepository.CreateAsync(new List<OzonClientModel> { clientModel });

                    manager.OzonEventHandler += str => {
                        OzonEventHandler?.Invoke(str);
                    };
                    managerList.Add(manager);

                    await manager.HandleOzonData(dto);
                }

                foreach (var item in managerList)
                {
                    item.OzonEventHandler -= str => {
                        OzonEventHandler?.Invoke(str);
                    };
                }
            }
            catch (Exception ex)
            {
                OzonEventHandler?.Invoke(ex.Message);
                throw;
            }

        }
    }
}