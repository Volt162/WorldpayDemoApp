using System.Diagnostics;
using Com.Paysimpleandroid.Paysimpleandroidsdk;
using Com.Paysimpleandroid.Paysimpleandroidsdk.Bbpos;
using Com.Paysimpleandroid.Paysimpleandroidsdk.Bbpos.PsEnums;
using Com.Paysimpleandroid.Paysimpleandroidsdk.Bbpos.PsListeners;
using Java.Interop;
using Java.Math;
using WpMAUIApp.Lib.Models;
using WpMAUIApp.Lib.Platforms.Android.Utils;
using WpMAUIApp.Lib.Services;

namespace WpMAUIApp.Lib.Platforms.Android.Services;

public class PaymentSdkService : Java.Lang.Object, IPaymentSdkService, IPsListener, IPsDeviceInteractionListener
{
    private const string SuccessSaleCode = "1000";
    private const string ApprovedSaleCode = "00";

    private IPaysimpleBBPOS? _psBBPOS;
    private TaskCompletionSource<OperationResult<MakeSaleResponse>> makeSaleTcs;

    public PaymentSdkService()
    {
        
    }

    #region -- IPsDeviceInteractionListener implementation -- 

    public void OnAmountConfirmation(PsAmountConfirmationType? p0, BigDecimal? p1, IPsDeviceInteractionListener.IPsConfirmAmountListener? p2)
    {
        p2?.PsConfirmAmount(true);
        DisplayTextChanged?.Execute("Thank you");
        AppLogger($"IPsDeviceInteractionListener OnAmountConfirmation p0={p0}, p1={p1}, p0={p2}");
    }

    public void OnCardRemoved()
    {
        AppLogger("IPsDeviceInteractionListener OnCardRemoved");
    }

    public void OnDeviceEvent(DeviceState? p0, string? p1)
    {
        DeviceStateChanged?.Execute(p0.Name());
        AppLogger($"IPsDeviceInteractionListener OnDeviceEvent p0={p0.Name()}, p1={p1}");
    }

    public void OnDisplayText(string? p0)
    {
        if (p0.Contains("timed out") ||
            String.Equals(p0, "Error completing sale: BBPos: Timeout - No Card Detected") ||
            p0.Contains("Error"))
        {
            DeviceStateChanged?.Execute("SaleFailed");
        }

        if (String.Equals(p0, "Error completing sale: BBPos: Timeout - No Card Detected") ||
            p0.Contains("Error"))
        {
            DisplayTextChanged?.Execute("No Card Detected");
        }
        else
        {
            DisplayTextChanged?.Execute(p0);
        }
        AppLogger($"IPsDeviceInteractionListener OnDisplayText p0={p0}");
    }

    public void OnError(PaysimpleSDKError? p0)
    {
        if (String.Equals(p0?.Description, "BBPOS: Bluetooth scan timed out.") ||
            String.Equals(p0?.Description, "Bluetooth (error code - 3029)") ||
            String.Equals(p0?.Description, "Heartbeat failed to connect to device after 10 attempts")) 
        {
            DeviceStateChanged?.Execute("ConnectingFailed");
        }
        AppLogger($"IPsDeviceInteractionListener OnError p0={p0?.Description}");
    }

    public void OnPaymentResponse(PaysimpleSaleResponse? p0)
    {
        OperationResult<MakeSaleResponse> result = new();
        if (p0 != null)
        {
            var makeSaleResponse = new MakeSaleResponse
            {
                AcquirerMessage = p0?.AcquirerMessage,
                AuthorizationCode = p0?.AuthorizationCode,
                BatchId = p0?.BatchId?.ToString(),
                Description = p0?.Description,
                EntryMode = p0?.EntryMode,
                Card = new SaleCardData
                {
                    CardBrand = p0?.PaysimpleCardData?.CardBrand,
                    CardHolderName = p0?.PaysimpleCardData?.CardHolderName,
                    ExpirationMonth = p0?.PaysimpleCardData?.ExpirationMonth,
                    ExpirationYear = p0?.PaysimpleCardData?.ExpirationYear,
                    Last4 = p0?.PaysimpleCardData?.Last4,
                },
                TransactionId = p0?.TransactionId,
                Outcome = new SaleOutcome
                {
                    Code = p0?.PaysimpleSaleOutcome?.Code,
                    OutcomeDescription = p0?.PaysimpleSaleOutcome?.OutcomeDescription,
                    Result = p0?.PaysimpleSaleOutcome?.Result,
                },
                Evm = new EmvData
                {
                    ApplicationCryptogram = p0?.PaysimpleEmvResponse?.Cryptogram,
                    ApplicationIdentifier = p0?.PaysimpleEmvResponse?.ApplicationIdentifier,
                    ApplicationLabel = p0?.PaysimpleEmvResponse?.ApplicationLabel,
                    ApplicationPreferredName = p0?.PaysimpleEmvResponse?.ApplicationPreferredName,
                    AuthorizationResponseCode = p0?.PaysimpleEmvResponse?.HostResponseCode,
                }
            };

            var isSuccess = string.Equals(p0?.PaysimpleSaleOutcome?.Code, SuccessSaleCode);
            var isApproved = string.Equals(p0?.PaysimpleSaleOutcome?.Code, ApprovedSaleCode) && string.Equals(p0?.PaysimpleSaleOutcome?.Result?.ToLower(), "approved");
            if (isSuccess || isApproved)
            {
                result.SetSuccess(makeSaleResponse);
            }
            else
            {
                result.SetFailure($"MakeSaleAsync failed. Status {p0?.PaysimpleSaleOutcome?.Result?.ToLower()}");
            }
        }
        else
        {
            result.SetFailure("PaysimpleSaleResponse is null");
            AppLogger("PaysimpleSaleResponse is null");
        }

        AppLogger($"OnPaymentResponse, result = {p0?.PaysimpleSaleOutcome?.Result}, TransactionId = {p0?.TransactionId}, code = {p0?.PaysimpleSaleOutcome?.Code}, description = {p0?.PaysimpleSaleOutcome?.OutcomeDescription}");
        DeviceStateChanged?.Execute("SaleCompleted");
        makeSaleTcs?.TrySetResult(result);
    }

    public void OnPromptUserForCard(string? p0)
    {
        DisplayTextChanged?.Execute(p0);
        AppLogger($"IPsDeviceInteractionListener OnPromptUserForCard p0={p0}");
    }

    public void OnRemoveCard()
    {
        DisplayTextChanged?.Execute("Please remove card");
        AppLogger("IPsDeviceInteractionListener OnRemoveCard");
    }

    public void SetJniIdentityHashCode(int value)
    {
        AppLogger($"IPsDeviceInteractionListener SetJniIdentityHashCode value={value}");
    }

    public void SetJniManagedPeerState(JniManagedPeerStates value)
    {
        AppLogger($"IPsDeviceInteractionListener SetJniManagedPeerState value={value}");
    }

    public void SetPeerReference(JniObjectReference reference)
    {
        AppLogger($"IPsDeviceInteractionListener SetPeerReference reference={reference}");
    }

    public void UnregisterFromRuntime()
    {
        AppLogger("IPsDeviceInteractionListener UnregisterFromRuntime");
    }

    public void UpdateResponse(Java.Lang.Double? p0, string[]? p1)
    {
        AppLogger($"IPsDeviceInteractionListener UpdateResponse p0={p0}, p1={p1?.First()}");
    }

    #endregion

    public Command<string> LogMessageChanged { get; set; }
    public Command<string> DeviceStateChanged { get; set; }
    public Command<string> DisplayTextChanged { get; set; }

    public bool IsInitialized => _psBBPOS?.IsSdkInitialized ?? false;
    public bool IsDeviceConnected => _psBBPOS?.IsDeviceConnected ?? false;

    public Task<OperationResult<bool>> IntitializeSdkAsync(string enviroment, string token)
    {
        AppLogger("Start psBBPOS Get new instance");
        OperationResult<bool> result = new();
        _psBBPOS = PaysimpleBBPOS.GetInstance(Platform.CurrentActivity, enviroment.GetPsEnvironment(), PsDeviceType.BbposEmv);
        _psBBPOS?.SubscribeToListener(this);
        _psBBPOS?.SubscribeToDeviceInteractionListener(this);

        if (_psBBPOS != null)
        {
            result.SetSuccess(true);
        }
        else
        {
            var mess = "_psBBPOS instance creation failed";
            AppLogger(mess);
            result.SetFailure(mess);
        }

        if (result.OperationSucceeded)
        {
            AppLogger("End psBBPOS Get new instance");
        }

        return Task.FromResult<OperationResult<bool>>(result);
        //return Task.Run(() =>
        //{
        //    AppLogger("Start initialization of sdk");
        //    try
        //    {
        //        _psBBPOS = PaysimpleBBPOS.GetInstance(Platform.CurrentActivity, enviroment.GetPsEnvironment(), PsDeviceType.BbposEmv);
        //        _psBBPOS?.SubscribeToListener(this);
        //        _psBBPOS?.SubscribeToDeviceInteractionListener(this);

        //        if (_psBBPOS != null)
        //        {
        //            var response = _psBBPOS.InitializeAndConnect(token, "CHB202044007347");
        //            AppLogger($"{response.Description}");
        //            if (_psBBPOS.IsSdkInitialized)
        //            {
        //                AppLogger("SDK is now officially initialized");
        //                result.SetSuccess(true);
        //            }
        //            else
        //            {
        //                var mess = "SDK failed to initialize";
        //                AppLogger(mess);
        //                result.SetFailure(mess);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var mess = $"Exception InitializeAndConnect: {ex.Message}";
        //        AppLogger(mess);
        //        result.SetFailure(mess);
        //    }

        //    AppLogger("End initialization of sdk");
        //    return Task.FromResult<OperationResult<bool>>(result);
        //});
    }

    public Task<OperationResult<IEnumerable<PaymentDevice>>> DiscoverDevicesAsync(string token)
    {
        OperationResult<IEnumerable<PaymentDevice>> result = new();
        return Task.Run(() =>
        {
            AppLogger("Start discovering devices");
            try
            {
                if (_psBBPOS != null)
                {
                    var response = _psBBPOS.GetAvailableDevices();
                    if (response?.DidSucceed() ?? false)
                    {
                        var paymentDevices = response!.DeviceNames?.Select(n => new PaymentDevice
                        {
                            IdNumber = n,
                            Manufacturer = n,
                        });
                        result.SetSuccess(paymentDevices);
                    }
                    else
                    {
                        result.SetFailure(response?.Description ?? "DiscoverDevicesAsync failed");
                        AppLogger(response?.Description ?? "DiscoverDevicesAsync failed");
                    }
                }
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

            return Task.FromResult<OperationResult<IEnumerable<PaymentDevice>>>(result);
        });
    }

    public Task<OperationResult<PaymentDevice>> ConnectDeviceAsync(string deviceId, string token)
    {
        OperationResult<PaymentDevice> result = new();
        return Task.Run(() =>
        {
            AppLogger("Start initialization of sdk and connect device");
            try
            {
                if (_psBBPOS != null)
                {
                    var response = _psBBPOS.InitializeAndConnect(token, deviceId);
                    AppLogger($"{response?.Description}");
                    if (_psBBPOS.IsSdkInitialized)
                    {
                        AppLogger("SDK is now officially initialized");
                        var paymentDevice = new PaymentDevice();
                        result.SetSuccess(paymentDevice);
                    }
                    else
                    {
                        var mess = "SDK failed to initialize";
                        AppLogger(mess);
                        result.SetFailure(mess);
                    }
                }
            }
            catch (Exception ex)
            {
                var mess = $"InitializeAndConnect failed and report on error: {ex.Message}";
                AppLogger(mess);
                result.SetFailure(mess);
            }

            if (result.OperationSucceeded)
            {
                AppLogger("End initialization of sdk");
            }

            return result;
        });
    }

    public Task<OperationResult<bool>> DisconnectDeviceAsync()
    {
        OperationResult<bool> result = new();
        return Task.Run(() =>
        {
            AppLogger("Start disconnecting device");
            try
            {
                if (_psBBPOS != null)
                {
                    _psBBPOS.DeInitializeBBPOS();
                    if (!_psBBPOS.IsDeviceConnected)
                    {
                        result.SetSuccess(true);
                    }
                    else
                    {
                        result.SetFailure("DisconnectDeviceAsync failed");
                        AppLogger("DisconnectDeviceAsync failed");
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
        });
    }

    public Task<OperationResult<PaymentDevice>> GetDeviceAsync()
    {
        OperationResult<PaymentDevice> result = new();
        AppLogger("Start getting device info");
        try
        {
            var deviceInfo = _psBBPOS!.GetDeviceInfo();
            if (deviceInfo?.Count > 0)
            {
                var paymentDevice = new PaymentDevice()
                {
                    IdNumber = deviceInfo?.First() ?? string.Empty,
                    Manufacturer = deviceInfo[1],
                    Model = deviceInfo?.Last() ?? string.Empty,
                };
                result.SetSuccess(paymentDevice);
            }
            else
            {
                result.SetFailure("GetDeviceAsync failed");
                AppLogger("GetDeviceAsync failed");
            }
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

    public Task<OperationResult<MakeSaleResponse>> MakeSaleAsync(MakeSaleParams makeSaleParams, string token)
    {
        OperationResult<MakeSaleResponse> result = new(true);
        makeSaleTcs = new TaskCompletionSource<OperationResult<MakeSaleResponse>>();
        return Task.Run( async() =>
        {
            AppLogger("Start make sale");
            try
            {
                if (_psBBPOS != null)
                {
                    var response = _psBBPOS.ProcessSale(token, makeSaleParams.ToNativeSaleParams());
                    if (!(response != null && response.DidSucceed()))
                    {
                        result.SetFailure($"MakeSaleAsync failed. Status {response?.Description}");
                        AppLogger($"MakeSaleAsync failed. Status {response?.Description}");
                    }
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
                result = await makeSaleTcs.Task;
            }

            if (result.OperationSucceeded)
            {
                AppLogger("End make sale");
            }

            return result;
        });
    }

    public Task<OperationResult<bool>> ResetSdkAsync()
    {
        OperationResult<bool> result = new();
        AppLogger("Start resetting of SDK");
        try
        {
            var response = _psBBPOS?.HardReset();
            if (response?.DidSucceed() ?? false)
            {
                result.SetSuccess(true);
            }
            else
            {
                result.SetFailure($"ResetSdkAsync failed. Status {response?.Description}");
                AppLogger($"ResetSdkAsync failed. Status {response?.Description}");
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

        return Task.FromResult<OperationResult<bool>>(result); ;
    }

    public Task<OperationResult<bool>> StopCurrentFlowAsync()
    {
        OperationResult<bool> result = new(true);
        return Task.Run(() =>
        {
            AppLogger("Start stopping current flow");
            try
            {
                if (_psBBPOS != null)
                {
                    var isSucceed = _psBBPOS.CancelCurrentAction();
                    if (isSucceed)
                    {
                        result.SetSuccess(true);
                    }
                    else
                    {
                        result.SetFailure("StopCurrentFlowAsync failed");
                        AppLogger("StopCurrentFlowAsync failed");
                    }
                }
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

            return result;
        });
    }

    private void AppLogger(string mess)
    {
        Debug.WriteLine(mess);
        LogMessageChanged?.Execute(mess);
    }
}