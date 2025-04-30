using System.Diagnostics;
using Foundation;
using PsMobileSDKBindingsLib;
using WpMAUIApp.Lib.Models;
using WpMAUIApp.Lib.Platforms.iOS.Utils;
using WpMAUIApp.Lib.Services;

namespace WpMAUIApp.Lib.Platforms.iOS.Services;

public class PaymentSdkService : IPaymentSdkService
{
    private const string SuccessSaleCode = "1000";

    private PsSDK _psSdkSharedInstance;

    public PaymentSdkService()
    {

    }

    public bool IsInitialized => _psSdkSharedInstance is not null;//_psSdkSharedInstance?.IsInitialized ?? false;
    public bool IsDeviceConnected => _psSdkSharedInstance?.IsDeviceConnected ?? false;

    public Command<string> DeviceStateChanged { get; set; }
    public Command<string> LogMessageChanged { get; set; }
    public Command<string> DisplayTextChanged { get; set; }

    public Task<OperationResult<bool>> IntitializeSdkAsync(string enviroment, string token)
    {
        OperationResult<bool> result = new();
        NSError error;
        AppLogger("Start initialization of SDK");
        if (PsSDK.InitializeSdk((uint)PsMobileSDKBindingsLib.DeviceType.Bbpos, enviroment, out error))
        {
            //_psSdkSharedInstance = new PsSDK()
            //{
            //    Delegate = new PsDelegate(DeviceStateChanged, LogMessageChanged)
            //};
            _psSdkSharedInstance = PsSDK.SharedInstance;
            _psSdkSharedInstance.Delegate = new PsDelegate(DeviceStateChanged, LogMessageChanged, DisplayTextChanged);
            AppLogger("SDK is now officially initialized");
            result.SetSuccess(true);
        }
        else
        {
            var mess = $"SDK failed to initialize and report on error: {error.LocalizedDescription}";
            AppLogger(mess);
            result.SetFailure(mess);
        }

        if (result.OperationSucceeded)
        {
            AppLogger("End initialization of SDK");
        }

        return Task.FromResult<OperationResult<bool>>(result);
    }

    public async Task<OperationResult<IEnumerable<PaymentDevice>>> DiscoverDevicesAsync(string token)
    {
        OperationResult<IEnumerable<PaymentDevice>> result = new();
        AppLogger("Start discovering devices");
        try
        {
            var response = await _psSdkSharedInstance.ScanForBluetoothDevicesWithClientTokenAsync(token);
            var devices = response;
            var paymentDevices = devices.Select(d => new PaymentDevice
            {
                IdNumber = d.Serial_number,
                Manufacturer = d.Manufacturer,
                Model = d.Model,
            });
            result.SetSuccess(paymentDevices);
        }
        catch (Exception ex)
        {
            var mess = $"DiscoverDevicesAsync failed and report on error: {ex.Message}";
            AppLogger(mess);
            result.SetFailure(mess);
        }

        if (result.OperationSucceeded)
        {
            AppLogger("End discovering devices");
        }

        return result;
    }

    public async Task<OperationResult<PaymentDevice>> ConnectDeviceAsync(string deviceId, string token)
    {
        OperationResult<PaymentDevice> result = new();
        AppLogger("Start connecting to device");
        try
        {
            var response = await _psSdkSharedInstance.ConnectDeviceWithClientTokenAsync(token, deviceId);
            if (response.Arg1)
            {
                var paymentDevice = new PaymentDevice
                {
                    IdNumber = response.Arg2.Serial_number,
                    Model = response.Arg2.Device_description,
                    Manufacturer = response.Arg2.Description,
                    BatteryPercentage = response.Arg2.Battery_percentage
                };
                result.SetSuccess(paymentDevice);
            }
            else
            {
                result.SetFailure("ConnectDeviceAsync failed");
                AppLogger("ConnectDeviceAsync failed");
            }
        }
        catch (Exception ex)
        {
            var mess = $"ConnectDeviceAsync failed and report on error: {ex.Message}";
            AppLogger(mess);
            result.SetFailure(mess);
        }

        if (result.OperationSucceeded)
        {
            AppLogger("End connecting to device");
        }

        return result;
    }

    public async Task<OperationResult<bool>> DisconnectDeviceAsync()
    {
        OperationResult<bool> result = new();
        AppLogger("Start disconnecting device");
        try
        {
            if (_psSdkSharedInstance != null)
            {
                var response = await _psSdkSharedInstance.DisconnectDeviceWithHandlerAsync();
                if (response.Item1)
                {
                    result.SetSuccess(response.Item1);
                }
                else
                {
                    result.SetFailure(response?.Item2?.LocalizedDescription ?? "DisconnectDeviceAsync failed");
                    AppLogger(response?.Item2?.LocalizedDescription ?? "DisconnectDeviceAsync failed");
                }
            }
        }
        catch (Exception ex)
        {
            var mess = $"DisconnectDeviceAsync failed and report on error: {ex.Message}";
            AppLogger(mess);
            result.SetFailure(mess);
        }

        if (result.OperationSucceeded)
        {
            AppLogger("End disconnecting device");
        }

        return result;
    }

    public Task<OperationResult<PaymentDevice>> GetDeviceAsync()
    {
        OperationResult<PaymentDevice> result = new();
        AppLogger("Start getting device info");
        try
        {
            var paymentDevice = new PaymentDevice
            {
                IdNumber = _psSdkSharedInstance.DeviceSerialNumber,
                Model = _psSdkSharedInstance.DeviceName,
                Manufacturer = _psSdkSharedInstance.Description,
                BatteryPercentage = _psSdkSharedInstance.DeviceBatteryPercentage,
            };
            result.SetSuccess(paymentDevice);
        }
        catch (Exception ex)
        {
            var mess = $"GetDeviceAsync failed and report on error: {ex.Message}";
            AppLogger(mess);
            result.SetFailure(mess);
        }

        if (result.OperationSucceeded)
        {
            AppLogger("End getting device info");
        }

        return Task.FromResult<OperationResult<PaymentDevice>>(result);
    }

    public async Task<OperationResult<MakeSaleResponse>> MakeSaleAsync(MakeSaleParams makeSaleParams, string token)
    {
        OperationResult<MakeSaleResponse> result = new();
        AppLogger("Start make sale");
        try
        {
            var response = await _psSdkSharedInstance.MakeSaleWithClientTokenAsync(token, makeSaleParams.ToNativeSaleParams());
            if (response != null)
            {
                var makeSaleResponse = new MakeSaleResponse
                {
                    AcquirerMessage = response?.Acquirer_message,
                    AuthorizationCode = response?.Authorization_code,
                    BatchId = response?.Batch_id?.ToString(),
                    Description = response?.Description,
                    EntryMode = response?.Entry_mode,
                    Card = new SaleCardData
                    {
                        CardBrand = response?.Card?.Card_brand,
                        CardHolderName = response?.Card?.Card_holder_name,
                        ExpirationMonth = response?.Card?.Expiration_month?.ToString(),
                        ExpirationYear = response?.Card?.Expiration_year?.ToString(),
                        Last4 = response?.Card?.Last4
                    },
                    TransactionId = response?.Transaction_id,
                    Outcome = new SaleOutcome
                    {
                        Code = response?.Outcome?.Code,
                        OutcomeDescription = response?.Outcome?.Outcome_description,
                        Result = response?.Outcome?.Result
                    },
                    Evm = new EmvData
                    {
                        ApplicationCryptogram = response?.Emv?.Cryptogram,
                        ApplicationIdentifier = response?.Emv?.Application_identifier,
                        ApplicationLabel = response?.Emv?.Application_label,
                        ApplicationPreferredName = response?.Emv?.Application_preferred_name,
                        AuthorizationResponseCode = response?.Emv?.Host_response_code,
                        AuthorizationResponseMessage = response?.Emv?.Host_response_message,
                    }
                };

                var isSuccess = string.Equals(response?.Outcome?.Code, SuccessSaleCode);
                if (isSuccess)
                {
                    result.SetSuccess(makeSaleResponse);
                }
                else
                {
                    result.SetFailure($"MakeSaleAsync failed. Status {response?.Outcome?.Result?.ToLower()}");
                    AppLogger($"MakeSaleAsync failed. Status {response?.Outcome?.Result?.ToLower()}");
                }
                AppLogger($"OnPaymentResponse, result = {response?.Outcome?.Result}, code = {response?.Outcome?.Code}, description = {response?.Outcome?.Description}");
            }
            else
            {
                result.SetFailure("PaysimpleSaleResponse is null");
                AppLogger("PaysimpleSaleResponse is null");
            }
        }
        catch (Exception ex)
        {
            var mess = $"MakeSaleAsync failed and report on error: {ex.Message}";
            AppLogger(mess);
            result.SetFailure(mess);
        }

        if (result.OperationSucceeded)
        {
            AppLogger("End make sale");
        }

        return result;
    }

    public async Task<OperationResult<bool>> ResetSdkAsync()
    {
        OperationResult<bool> result = new();
        AppLogger("Start resetting of SDK");
        try
        {
            var response = await DisconnectDeviceAsync();
            if (response.OperationSucceeded || !_psSdkSharedInstance.IsDeviceConnected)
            {
                _psSdkSharedInstance.ResetSdk();
                _psSdkSharedInstance = null;
                result.SetSuccess(true);
            }
            else
            {
                result.SetFailure($"ResetSdkAsync failed and {response.Message}");
                AppLogger($"ResetSdkAsync failed and {response.Message}");
            }
        }
        catch (Exception ex)
        {
            var mess = $"ResetSdkAsync failed and report on error: {ex.Message}";
            AppLogger(mess);
            result.SetFailure(mess);
        }

        if (result.OperationSucceeded)
        {
            AppLogger("End resetting of SDK");
        }

        return result;
    }

    public Task<OperationResult<bool>> StopCurrentFlowAsync()
    {
        OperationResult<bool> result = new();
        AppLogger("Start stopping current flow");
        try
        {
            _psSdkSharedInstance.StopCurrentBbposFlow();
            result.SetSuccess(true);
        }
        catch (Exception ex)
        {
            var mess = $"StopCurrentFlowAsync failed and report on error: {ex.Message}";
            AppLogger(mess);
            result.SetFailure(mess);
        }

        if (result.OperationSucceeded)
        {
            AppLogger("End stopping current flow");
        }

        return Task.FromResult<OperationResult<bool>>(result);
    }

    private void AppLogger(string mess)
    {
        Debug.WriteLine(mess);
        LogMessageChanged?.Execute(mess);
    }
}
