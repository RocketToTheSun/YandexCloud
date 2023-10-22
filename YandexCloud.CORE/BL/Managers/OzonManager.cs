using AutoMapper;
using System.Collections;
using System.Linq;
using YandexCloud.CORE.DTOs;
using YandexCloud.CORE.Models;
using YandexCloud.CORE.Repositories;

namespace YandexCloud.CORE.BL.Managers
{
    public class OzonManager : IOzonManager
    {
        readonly IOzonFullData _ozonFullData;
        readonly IUoW _uoW;
        public event Action<string> OzonEventHandler;

        public OzonManager(IOzonFullData ozon, IUoW uoW)
        {
            _ozonFullData = ozon;
            _uoW = uoW;
        }

        public async Task HandleOzonData(RequestDataDto requestDto)
        {
            await _uoW.OpenTransactionAsync();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<RequestDataDto, RequestDataModel>()).CreateMapper();
            var requestModel = mapper.Map<RequestDataDto, RequestDataModel>(requestDto);
            requestModel.OperationType = "OperationAgentDeliveredToCustomer";

            try
            {
                OzonEventHandler?.Invoke("Начинаем получать данные из озона");
                var ozonData = new List<OzonResponseModel>();

                ozonData.Add(await _ozonFullData.GetDeliveryDataAsync(requestModel));

                var pageCount = ozonData.First().result.page_count;

                for (int i = 2; i <= pageCount; i++)
                {
                    requestModel.Page = i;
                    ozonData.Add(await _ozonFullData.GetDeliveryDataAsync(requestModel));
                }

                var items = new List<List<Item>>();

                foreach (var responseModel in ozonData)
                {
                    foreach (var item in responseModel.result.operations)
                    {
                        items.Add(item.items);
                    }
                }

                var skus = new List<int>();
                
                foreach (var itemList in items)
                {
                    foreach (var item in itemList)
                    {
                        skus.Add(item.sku);
                    }
                }
                
                var filteredSkus = skus.Distinct().ToList();

                var requestArticuls = new RequestArticulsDto 
                {
                    ClientId = requestModel.ClientId,
                    ApiKey = requestModel.ApiKey,
                    sku = filteredSkus,
                };
                var articulsModel = await _ozonFullData.GetArticulsAsync(requestArticuls);

                var checkSkuListFromBD = await _uoW.OzonArticuslData.ReadAsync();

                var uniqueInListFromOzon = new List<DTOs.Articuls.Item>();

                foreach (var skuBD in articulsModel.result.items)
                {
                    if (skuBD.offer_id == "1044865213")
                    {
                        Console.WriteLine(skuBD.offer_id);
                    }
                    if (!checkSkuListFromBD.Any(s => s.offer_id == skuBD.offer_id))
                    {
                        uniqueInListFromOzon.Add(skuBD);
                    }
                    else
                    {
                        if (!checkSkuListFromBD.Any(s => s.fbo_sku == skuBD.fbo_sku))
                        {
                            uniqueInListFromOzon.Add(skuBD);

                            if (!checkSkuListFromBD.Any(s => s.fbs_sku == skuBD.fbs_sku)) uniqueInListFromOzon.Add(skuBD);
                        }
                    }

                }

                await _uoW.OzonArticuslData.CreateAsync(uniqueInListFromOzon);


                var ozonServiseNames = await _uoW.OzonServiceNamesRepository.GetAsync();


                var ozonFirstDataList = new List<OzonFirstTableModel>();
                var ozonSecondDataList = new List<OzonSecondDataDto>();

                foreach (var item in ozonData)
                {
                    foreach (var data in item.result.operations)
                    {
                        var id = Guid.NewGuid();
                        ozonFirstDataList.Add(new OzonFirstTableModel
                        {
                            id = id.ToString(),
                            date = Convert.ToDateTime(data.operation_date),
                            sku = data.items[0].sku.ToString(),
                            name = data.items[0].name,
                            posting_number = data.posting.posting_number,
                            accruals_for_sale = data.accruals_for_sale.Value,
                            sale_comission = data.sale_commission,
                            clients_id = requestDto.ClientId
                        });

                        var service = ozonServiseNames.FirstOrDefault();
                        var serviceFromResponse = data.services.FirstOrDefault(n => n.name == service.name);

                        ozonSecondDataList.Add(new OzonSecondDataDto
                        {
                            first_table_id = id.ToString(),
                            price = serviceFromResponse?.price,
                            ozon_service_names_id = 1
                        });

                        service = ozonServiseNames.FirstOrDefault(i => i.id == 2);
                        serviceFromResponse = data.services.FirstOrDefault(n => n.name == service.name);

                        ozonSecondDataList.Add(new OzonSecondDataDto
                        {
                            first_table_id = id.ToString(),
                            price = serviceFromResponse?.price,
                            ozon_service_names_id = 2
                        });
                    }
                }

                await _uoW.OzonMainDataRepository.CreateAsync(ozonFirstDataList);
                await _uoW.OzonSecondDataRepository.CreateAsync(ozonSecondDataList);

                requestModel.OperationType = "MarketplaceRedistributionOfAcquiringOperation";
                requestModel.Page = 1;
                ozonData.Clear();
                var ozonAcquiringData = new List<OzonAcquiringDataDto>();

                for (int i = 1; i <= pageCount; i++)
                {
                    requestModel.Page = i;
                    ozonData.Add(await _ozonFullData.GetDeliveryDataAsync(requestModel));
                }

                foreach (var item in ozonData)
                {
                    foreach (var data in item.result.operations)
                    {
                        ozonAcquiringData.Add(new OzonAcquiringDataDto
                        {
                            sku = data.items.FirstOrDefault().sku.ToString(),
                            name = data.items.FirstOrDefault().name,
                            amount = data.amount,
                            posting_number = data.posting.posting_number,
                            date = Convert.ToDateTime(data.operation_date),
                            operation_id = data.operation_id.ToString(),
                            clients_id = requestDto.ClientId
                        });
                    }
                }

                await _uoW.OzonAcquiringRepository.CreateAsync(ozonAcquiringData);

                requestModel.OperationType = "MarketplaceMarketingActionCostOperation";
                requestModel.Page = 1;
                ozonData.Clear();
                var ozonMarketingActionData = new List<OzonMarketingActionCostModel>();

                for (int i = 1; i <= pageCount; i++)
                {
                    requestModel.Page = i;
                    ozonData.Add(await _ozonFullData.GetDeliveryDataAsync(requestModel));
                }

                foreach (var item in ozonData)
                {
                    foreach (var data in item.result.operations)
                    {
                        ozonMarketingActionData.Add(new OzonMarketingActionCostModel
                        {
                            amount = data.amount,
                            date = Convert.ToDateTime(data.operation_date),
                            operation_id = data.operation_id.ToString(),
                            clients_id = requestDto.ClientId
                        });
                    }
                }
                await _uoW.OzonMarketingActionsRepository.CreateAsync(ozonMarketingActionData);


                requestModel.OperationType = "ClientReturnAgentOperation";
                requestModel.Page = 1;
                ozonData.Clear();
                var clientReturnAgentData = new List<ClientReturnAgentOperationModel>();

                for (int i = 1; i <= pageCount; i++)
                {
                    requestModel.Page = i;
                    ozonData.Add(await _ozonFullData.GetDeliveryDataAsync(requestModel));
                }

                foreach (var item in ozonData)
                {
                    foreach (var data in item.result.operations)
                    {
                        clientReturnAgentData.Add(new ClientReturnAgentOperationModel
                        {
                            sku = data.items.FirstOrDefault().sku.ToString(),
                            name = data.items.FirstOrDefault().name,
                            amount = data.amount,
                            posting_number = data.posting.posting_number,
                            date = Convert.ToDateTime(data.operation_date),
                            operation_id = data.operation_id.ToString(),
                            clients_id = requestDto.ClientId
                        });
                    }
                }

                requestModel.OperationType = "ReturnAgentOperationRFBS";
                requestModel.Page = 1;
                ozonData.Clear();
                var returnAgentOperationRFBSData = new List<ReturnAgentOperationRFBSModel>();

                for (int i = 1; i <= pageCount; i++)
                {
                    requestModel.Page = i;
                    ozonData.Add(await _ozonFullData.GetDeliveryDataAsync(requestModel));
                }

                foreach (var item in ozonData)
                {
                    foreach (var data in item.result.operations)
                    {
                        returnAgentOperationRFBSData.Add(new ReturnAgentOperationRFBSModel
                        {
                            sku = data.items.FirstOrDefault().sku.ToString(),
                            name = data.items.FirstOrDefault().name,
                            amount = data.amount,
                            posting_number = data.posting.posting_number,
                            date = Convert.ToDateTime(data.operation_date),
                            operation_id = data.operation_id.ToString(),
                            clients_id = requestDto.ClientId
                        });
                    }
                }

                await _uoW.OzonClientReturnAgentRepository.CreateAsync(clientReturnAgentData);
                await _uoW.OzonReturnAgentOperationRFBSRepository.CreateAsync(returnAgentOperationRFBSData);


                requestModel.OperationType = "OperationReturnGoodsFBSofRMS";
                requestModel.Page = 1;
                ozonData.Clear();
                var operationReturnGoodsFBSofRMSData = new List<OperationReturnGoodsFBSofRMSModel>();
                var priceByReturnGoodsFBSOfRMSData = new List<PriceByReturnGoodsFBSOfRMSModel>();

                for (int i = 1; i <= pageCount; i++)
                {
                    requestModel.Page = i;
                    ozonData.Add(await _ozonFullData.GetDeliveryDataAsync(requestModel));
                }

                foreach (var item in ozonData)
                {
                    foreach (var data in item.result.operations)
                    {
                        var operationReturnGoodsFBSofRMSDataGuid = Guid.NewGuid().ToString();

                        var serviceDelivToCustomer = data.services.FirstOrDefault(s => s.name == "MarketplaceServiceItemReturnAfterDelivToCustomer");
                        var serviceFlowLogistic = data.services.FirstOrDefault(s => s.name == "MarketplaceServiceItemReturnFlowLogistic");
                        var serviceNotDelivToCustomer = data.services.FirstOrDefault(s => s.name == "MarketplaceServiceItemReturnNotDelivToCustomer");
                        var serviceReturnPartGoodsCustomer = data.services.FirstOrDefault(s => s.name == "MarketplaceServiceItemReturnPartGoodsCustomer");

                        if (serviceDelivToCustomer != null)
                            priceByReturnGoodsFBSOfRMSData.Add(new PriceByReturnGoodsFBSOfRMSModel
                            {
                                price = serviceDelivToCustomer.price,
                                ozon_service_name_id = 3,
                                operation_return_goods_fbsof_rms_id = operationReturnGoodsFBSofRMSDataGuid,
                            });

                        if (serviceFlowLogistic != null)
                            priceByReturnGoodsFBSOfRMSData.Add(new PriceByReturnGoodsFBSOfRMSModel
                            {
                                price = serviceFlowLogistic.price,
                                ozon_service_name_id = 4,
                                operation_return_goods_fbsof_rms_id = operationReturnGoodsFBSofRMSDataGuid,
                            });

                        if (serviceNotDelivToCustomer != null)
                            priceByReturnGoodsFBSOfRMSData.Add(new PriceByReturnGoodsFBSOfRMSModel
                            {
                                price = serviceNotDelivToCustomer.price,
                                ozon_service_name_id = 5,
                                operation_return_goods_fbsof_rms_id = operationReturnGoodsFBSofRMSDataGuid,
                            });

                        if (serviceReturnPartGoodsCustomer != null)
                            priceByReturnGoodsFBSOfRMSData.Add(new PriceByReturnGoodsFBSOfRMSModel
                            {
                                price = serviceReturnPartGoodsCustomer.price,
                                ozon_service_name_id = 6,
                                operation_return_goods_fbsof_rms_id = operationReturnGoodsFBSofRMSDataGuid,
                            });

                        operationReturnGoodsFBSofRMSData.Add(new OperationReturnGoodsFBSofRMSModel
                        {
                            id = operationReturnGoodsFBSofRMSDataGuid,
                            sku = data.items.FirstOrDefault().sku.ToString(),
                            name = data.items.FirstOrDefault().name,
                            amount = data.amount,
                            posting_number = data.posting.posting_number,
                            date = Convert.ToDateTime(data.operation_date),
                            operation_id = data.operation_id.ToString(),
                            clients_id = requestDto.ClientId
                        });
                    }
                }

                await _uoW.OzonOperationReturnGoodsFbsOfRmsRepository.CreateAsync(operationReturnGoodsFBSofRMSData);
                await _uoW.OzonPriceByReturnGoodsFbsOfRmsRepository.CreateAsync(priceByReturnGoodsFBSOfRMSData);

                requestModel.OperationType = "OperationItemReturn";
                requestModel.Page = 1;
                ozonData.Clear();
                var operationItemReturnData = new List<OperationItemReturnModel>();
                var priceByOperationItemReturnData = new List<PriceByOperationItemReturnModel>();

                for (int i = 1; i <= pageCount; i++)
                {
                    requestModel.Page = i;
                    ozonData.Add(await _ozonFullData.GetDeliveryDataAsync(requestModel));
                }

                foreach (var item in ozonData)
                {
                    foreach (var data in item.result.operations)
                    {
                        var operationReturnGoodsFBSofRMSDataGuid = Guid.NewGuid().ToString();

                        var serviceDelivToCustomer = data.services.FirstOrDefault(s => s.name == "MarketplaceServiceItemReturnAfterDelivToCustomer");
                        var serviceFlowLogistic = data.services.FirstOrDefault(s => s.name == "MarketplaceServiceItemReturnFlowLogistic");
                        var serviceNotDelivToCustomer = data.services.FirstOrDefault(s => s.name == "MarketplaceServiceItemReturnNotDelivToCustomer");
                        var serviceReturnPartGoodsCustomer = data.services.FirstOrDefault(s => s.name == "MarketplaceServiceItemReturnPartGoodsCustomer");

                        if (serviceDelivToCustomer != null)
                            priceByOperationItemReturnData.Add(new PriceByOperationItemReturnModel
                            {
                                price = serviceDelivToCustomer.price,
                                ozon_service_name_id = 3,
                                operation_item_return_id = operationReturnGoodsFBSofRMSDataGuid,
                            });

                        if (serviceFlowLogistic != null)
                            priceByOperationItemReturnData.Add(new PriceByOperationItemReturnModel
                            {
                                price = serviceFlowLogistic.price,
                                ozon_service_name_id = 4,
                                operation_item_return_id = operationReturnGoodsFBSofRMSDataGuid,
                            });

                        if (serviceNotDelivToCustomer != null)
                            priceByOperationItemReturnData.Add(new PriceByOperationItemReturnModel
                            {
                                price = serviceNotDelivToCustomer.price,
                                ozon_service_name_id = 5,
                                operation_item_return_id = operationReturnGoodsFBSofRMSDataGuid,
                            });

                        if (serviceReturnPartGoodsCustomer != null)
                            priceByOperationItemReturnData.Add(new PriceByOperationItemReturnModel
                            {
                                price = serviceReturnPartGoodsCustomer.price,
                                ozon_service_name_id = 6,
                                operation_item_return_id = operationReturnGoodsFBSofRMSDataGuid,
                            });

                        operationItemReturnData.Add(new OperationItemReturnModel
                        {
                            id = operationReturnGoodsFBSofRMSDataGuid,
                            sku = data.items.FirstOrDefault().sku.ToString(),
                            name = data.items.FirstOrDefault().name,
                            amount = data.amount,
                            posting_number = data.posting.posting_number,
                            date = Convert.ToDateTime(data.operation_date),
                            operation_id = data.operation_id.ToString(),
                            clients_id = requestDto.ClientId
                        });
                    }
                }

                await _uoW.OzonOperationItemReturnRepository.CreateAsync(operationItemReturnData);
                await _uoW.OzonPriceByOperationItemReturnRepository.CreateAsync(priceByOperationItemReturnData);


                requestModel.OperationType = "OperationMarketplaceServicePremiumCashbackIndividualPoints";
                requestModel.Page = 1;
                ozonData.Clear();
                var premiumCashbackIndividualPointsData = new List<PremiumCashbackIndividualPointsModel>();

                for (int i = 1; i <= pageCount; i++)
                {
                    requestModel.Page = i;
                    ozonData.Add(await _ozonFullData.GetDeliveryDataAsync(requestModel));
                }

                foreach (var item in ozonData)
                {
                    foreach (var data in item.result.operations)
                    {
                        premiumCashbackIndividualPointsData.Add(new PremiumCashbackIndividualPointsModel
                        {
                            sku = data.items.FirstOrDefault().sku.ToString(),
                            name = data.items.FirstOrDefault().name,
                            amount = data.amount,
                            posting_number = data.posting.posting_number,
                            date = Convert.ToDateTime(data.operation_date),
                            operation_id = data.operation_id.ToString(),
                            clients_id = requestDto.ClientId
                        });
                    }
                }

                requestModel.OperationType = "OperationMarketplaceWithHoldingForUndeliverableGoods";
                requestModel.Page = 1;
                ozonData.Clear();
                var holdingForUndeliverableGoodsData = new List<HoldingForUndeliverableGoodsModel>();

                for (int i = 1; i <= pageCount; i++)
                {
                    requestModel.Page = i;
                    ozonData.Add(await _ozonFullData.GetDeliveryDataAsync(requestModel));
                }

                foreach (var item in ozonData)
                {
                    foreach (var data in item.result.operations)
                    {
                        holdingForUndeliverableGoodsData.Add(new HoldingForUndeliverableGoodsModel
                        {
                            sku = data.items.FirstOrDefault().sku.ToString(),
                            name = data.items.FirstOrDefault().name,
                            amount = data.amount,
                            posting_number = data.posting.posting_number,
                            date = Convert.ToDateTime(data.operation_date),
                            operation_id = data.operation_id.ToString(),
                            clients_id = requestDto.ClientId
                        });
                    }
                }

                await _uoW.OzonPremiumCashbackIndividualPointsRepository.CreateAsync(premiumCashbackIndividualPointsData);
                await _uoW.OzonHoldingForUndeliverableGoodsRepository.CreateAsync(holdingForUndeliverableGoodsData);

                OzonEventHandler?.Invoke("Сохранили обработанные данные");

                await _uoW.CommitAsync();
                OzonEventHandler?.Invoke("Данные успешно сохранены");

            }
            catch (Exception ex)
            {
                await _uoW.RollbackAsync();
                throw;
            }
        }
    }
}
