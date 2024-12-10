using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Domain.HubEntities;
using OnAim.Admin.Infrasturcture.Interfaces;

namespace OnAim.Admin.APP.Services.HubServices.Coin;

public class CoinService : ICoinService
{
    private readonly HubClientService _hubClientService;
    private readonly IReadOnlyRepository<WithdrawOption> _withdrawOptionRepository;
    private readonly IReadOnlyRepository<WithdrawOptionGroup> _withdrawOptionGroupRepository;

    public CoinService(
        HubClientService hubClientService,
        IReadOnlyRepository<WithdrawOption> withdrawOptionRepository,
        IReadOnlyRepository<WithdrawOptionGroup> withdrawOptionGroupRepository)
    {
        _hubClientService = hubClientService;
        _withdrawOptionRepository = withdrawOptionRepository;
        _withdrawOptionGroupRepository = withdrawOptionGroupRepository;
    }

    public async Task<IEnumerable<WithdrawOption>> GetWithdrawOptions(Domain.HubEntities.Models.CreateOutCoinModel? outCoinModel)
    {
        if (outCoinModel == null)
        {
            throw new Exception(
                "The provided model is null. Please ensure that a valid OutCoin model is supplied."
            );
        }

        var withdrawOptions = _withdrawOptionRepository.Query(wo => outCoinModel.WithdrawOptionIds.Any(woId => woId == wo.Id));

        if (withdrawOptions == null || !withdrawOptions.Any())
        {
            throw new Exception(
                "No withdraw options were found for the provided list of IDs. Please ensure the IDs are valid and correspond to existing segments."
            );
        }

        return withdrawOptions;
    }

    public async Task<IEnumerable<WithdrawOptionGroup>> GetWithdrawOptionGroups(Domain.HubEntities.Models.CreateOutCoinModel? outCoinModel)
    {
        if (outCoinModel == null)
        {
            throw new Exception(
                "The provided model is null. Please ensure that a valid OutCoin model is supplied."
            );
        }

        var withdrawOptionGroups = _withdrawOptionGroupRepository.Query(wog => outCoinModel.WithdrawOptionGroupIds.Any(wogId => wogId == wog.Id));

        if (withdrawOptionGroups == null || !withdrawOptionGroups.Any())
        {
            throw new Exception(
                "No withdraw option groups were found for the provided list of IDs. Please ensure the IDs are valid and correspond to existing segments."
            );
        }

        return withdrawOptionGroups;
    }

    public async Task<ApplicationResult> CreateWithdrawOption(CreateWithdrawOption option)
    {
        try
        {
            await _hubClientService.CreateWithdrawOptionAsync(option);

            return new ApplicationResult { Success = true };
        }
        catch (Exception)
        {

            throw new Exception("failed to save withdraw option");
        }
    }

    public async Task<ApplicationResult> UpdateWithdrawOption(UpdateWithdrawOption option)
    {
        try
        {
            await _hubClientService.UpdateWithdrawOptionAsync(option);

            return new ApplicationResult { Success = true };
        }
        catch (Exception)
        {

            throw new Exception("failed to update withdraw option");
        }
    }

    public async Task<ApplicationResult> CreateWithdrawOptionEndpoint(CreateWithdrawOptionEndpoint option)
    {
        try
        {
            await _hubClientService.CreateWithdrawOptionEndpointAsync(option);

            return new ApplicationResult { Success = true };
        }
        catch (Exception)
        {

            throw new Exception("failed to save withdraw option endpoint");
        }
    }

    public async Task<ApplicationResult> UpdateWithdrawOptionEndpoint(UpdateWithdrawOptionEndpoint option)
    {
        try
        {
            await _hubClientService.UpdateWithdrawOptionEndpointAsync(option);

            return new ApplicationResult { Success = true };
        }
        catch (Exception)
        {

            throw new Exception("failed to update withdraw option endpoint");
        }
    }

    public async Task<ApplicationResult> CreateWithdrawOptionGroup(CreateWithdrawOptionGroup option)
    {
        try
        {
            await _hubClientService.CreateWithdrawOptionGroupAsync(option);

            return new ApplicationResult { Success = true };
        }
        catch (Exception)
        {

            throw new Exception("failed to save withdraw option Group");
        }
    }

    public async Task<ApplicationResult> UpdateWithdrawOptionGroup(UpdateWithdrawOptionGroup option)
    {
        try
        {
            await _hubClientService.UpdateWithdrawOptionGroupAsync(option);

            return new ApplicationResult { Success = true };
        }
        catch (Exception)
        {

            throw new Exception("failed to update withdraw option Group");
        }
    }
}
