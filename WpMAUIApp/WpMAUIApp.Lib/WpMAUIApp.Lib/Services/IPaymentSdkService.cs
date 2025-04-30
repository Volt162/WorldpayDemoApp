using WpMAUIApp.Lib.Models;

namespace WpMAUIApp.Lib.Services;

public interface IPaymentSdkService : IDeviceStateService, IDisplayLogService, IDisplayTextService
{
    bool IsInitialized { get; }
    bool IsDeviceConnected { get; }
    Task<OperationResult<bool>> IntitializeSdkAsync(string enviroment, string token);
    Task<OperationResult<bool>> ResetSdkAsync();
    Task<OperationResult<IEnumerable<PaymentDevice>>> DiscoverDevicesAsync(string token);
    Task<OperationResult<PaymentDevice>> ConnectDeviceAsync(string deviceId, string token);
    Task<OperationResult<bool>> DisconnectDeviceAsync();
    Task<OperationResult<MakeSaleResponse>> MakeSaleAsync(MakeSaleParams makeSaleParams, string token);
    Task<OperationResult<PaymentDevice>> GetDeviceAsync();
    Task<OperationResult<bool>> StopCurrentFlowAsync();
}
