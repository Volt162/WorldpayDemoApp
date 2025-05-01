using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WpMAUIApp.Lib.Models;
using WpMAUIApp.Lib.Services;

namespace WpMAUIApp.ViewModels
{
    public partial class MainPageViewModel : BaseViewModel
    {
        private const string DeviceConnectedState = "Connected";
        private const string DeviceUpdatingState = "Updating"; 
        private const string DeviceUpdateFinishedState = "UpdateFinished";
        private const string DeviceDisconnectedState = "Disconnected";
        private const string DeviceConnectingFailedState = "ConnectingFailed";
        private const string DeviceSaleCompletedState = "SaleCompleted";
        private const string DeviceSaleFailedState = "SaleFailed";

        private readonly IPaymentSdkService _paymentSdkService;
        private string _token;

        public MainPageViewModel(IPaymentSdkService paymentSdkService)
        {
            _paymentSdkService = paymentSdkService;
            _paymentSdkService.DeviceStateChanged = new Command<string>(OnDeviceStateChanged);
            _paymentSdkService.LogMessageChanged = new Command<string>(OnLogMessageChanged);
            _paymentSdkService.DisplayTextChanged = new Command<string>(OnBBposDisplayTextChanged);
        }

        #region -- Properties --

        [ObservableProperty]
        private string _displayText;

        [ObservableProperty]
        private bool _isPaired;

        [ObservableProperty]
        private bool _isScanning;

        [ObservableProperty]
        private IList<PaymentDevice> _devices;

        [ObservableProperty]
        private bool _isConnected;

        [ObservableProperty]
        private bool _isInitialized;
        
        [ObservableProperty]
        private bool _isSalesProcessing;

        [ObservableProperty]
        private PaymentDevice _selectedDevice;

        [ObservableProperty]
        private decimal _amount = 0.00m;

        [ObservableProperty]
        private string _currentDeviceState = DeviceDisconnectedState;

        [ObservableProperty]
        private string _latestLogMessage;

        [ObservableProperty]
        private string _prevLogMessage; 

        [ObservableProperty]
        private string _pairedText = $"You already have a paired terminal {Preferences.Default.Get("DeviceIdNumber", string.Empty)} connection will be automatic. To cancel the pair and disconnect the terminal please Reset SDK";

        #endregion

        #region -- Commands --

        [RelayCommand]
        private async void DeviceSelected(PaymentDevice device)
        {
            if (IsConnected)
            {
                await Application.Current.MainPage.DisplayAlert("Warning", $"Please first you need to disconnect from {SelectedDevice.DisplaySubtitle} device", "Cancel");
            }
            else
            {
                var res = await Application.Current.MainPage.DisplayAlert("Connect to device", $"Do you want to connect to {device.DisplaySubtitle} device?", "Connect", "Cancel");
                if (res)
                {
                    await ConnectDeviceAsync(device);
                }
            }
        }

        [RelayCommand]
        private async void ScanDevices()
        {
            UpdateStatus();
            IsScanning = true;
            Devices = null;

            var token = await GetToken();
            var result = await _paymentSdkService.DiscoverDevicesAsync(token);
            if (result.OperationSucceeded)
            {
                Devices = result!.Result!.ToList();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Warning", result.Message, "Ok");
            }

            IsScanning = false;
        }

        [RelayCommand]
        private async Task<bool> InitSDKAsync()
        {
            bool res = false;
            ShowLoader("Init SDK...");
            var token = await GetToken();
            var result = await _paymentSdkService.IntitializeSdkAsync("SBX", token);
            if (result.OperationSucceeded)
            {
                res = true;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Warning", result.Message, "Ok");
            }
            UpdateStatus();
            HideLoader();

            return res;
        }

        [RelayCommand]
        private async Task ResetAsync()
        {
            var res = await Application.Current.MainPage.DisplayAlert("Reset SDK", $"Do you want to reset SDK and disconnect device?", "Reset", "Cancel");
            if (res)
            {
                ShowLoader("Reseting SDK...");
                var result = await _paymentSdkService.ResetSdkAsync();
                if (result.OperationSucceeded)
                {
                    _token = null;
                    SelectedDevice = null;
                    Devices = null;
                    DisplayText = null;
                    IsScanning = false;
                    IsPaired = false;
                    Amount = 0.00m;
                    Preferences.Default.Remove("DeviceIdNumber");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Warning", result.Message, "Ok");
                }
                UpdateStatus();
                HideLoader();
            }
        }

        [RelayCommand]
        private async Task ShowDeviceInfoAsync()
        {
            if (IsConnected && !IsSalesProcessing)
            {
                await GetDeviceInfoAsync();
                if (SelectedDevice != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Device Info", $"{SelectedDevice.DisplayTitle} {SelectedDevice.DisplaySubtitle}", "Ok");
                }

                UpdateStatus();
            }
        }

        [RelayCommand]
        private async void MakeSale()
        {
            var isEnable = IsInitialized && IsConnected && !IsSalesProcessing;
            if (!isEnable)
            {
                await Application.Current.MainPage.DisplayAlert("Warning", "The sale has already started", "Ok");
                return;
            }

            if (Amount > 0.00m)
            {
                //ShowLoader("Processing Sale...");
                var token = await GetToken();
                var param = new MakeSaleParams(true, Amount.ToString());
                IsSalesProcessing = true;
                var result = await _paymentSdkService.MakeSaleAsync(param, token);
                if (result.OperationSucceeded)
                {
                    if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
                    {
                        IsSalesProcessing = false;
                    }
                    DisplayText = null;
                    Amount = 0.00m;
                    await Application.Current.MainPage.DisplayAlert("", "Sale successful!", "Ok");
                }
                else
                {
                    IsSalesProcessing = false;
                    DisplayText = null;
                    Amount = 0.00m;
                    await Application.Current.MainPage.DisplayAlert("Warning", result.Message, "Ok");
                }
                UpdateStatus();
                //HideLoader();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Warning", "Amount should be greater than 0", "Ok");
            }
        }

        [RelayCommand]
        private async Task StopCurrentFlowAsync()
        {
            var res = await Application.Current.MainPage.DisplayAlert("Stop current flow", $"Do you want to stop the current BBPOS sale flow?", "Stop", "Cancel");
            if (res)
            {
                ShowLoader("Stopping Current Flow...");
                var result = await _paymentSdkService.StopCurrentFlowAsync();
                if (result.OperationSucceeded)
                {
                    await Application.Current.MainPage.DisplayAlert("", "Flow stopped successfully!", "Ok");
                    IsSalesProcessing = false;
                    DisplayText = null;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Warning", result.Message, "Ok");
                }
                UpdateStatus();
                HideLoader();
            }
        }

        #endregion

        public override Task OnInitializeAsync()
        {
            if (Preferences.Default.ContainsKey("DeviceIdNumber"))
            {
                IsPaired = true;
                InitAndConnectDeviceAsync(Preferences.Default.Get("DeviceIdNumber", string.Empty));
            }
            else
            {
                IsPaired = false;
                InitSDKAsync();
            }
            
            return base.OnInitializeAsync();
        }

        #region -- Private helpers --

        private void OnBBposDisplayTextChanged(string text)
        {
            DisplayText = text;
        }

        private async void OnDeviceStateChanged(string state)
        {
            switch (state)
            {
                case DeviceConnectedState:
                    await GetDeviceInfoAsync();
                    Preferences.Default.Set("DeviceIdNumber", SelectedDevice.IdNumber);
                    IsPaired = true;
                    HideLoader();
                    break;
                case DeviceUpdatingState:
                    ShowLoader("Updating...");
                    break;
                case DeviceConnectingFailedState:
                    ReInitSdk();
                    HideLoader();
                    IsPaired = false;
                    IsScanning = false;
                    break;
                case DeviceSaleFailedState:
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        IsSalesProcessing = false;
                        DisplayText = null;
                        await Application.Current.MainPage.DisplayAlert("Warning", "MakeSaleAsync failed", "Ok");
                    });
                    break;
                case DeviceDisconnectedState:
                    ReInitSdk();
                    IsPaired = false;
                    HideLoader();
                    break;
                case DeviceUpdateFinishedState:
                    HideLoader();
                    break;
                case DeviceSaleCompletedState:
                    IsSalesProcessing = false;
                    break;
                default:
                    break;
            }

            if (!(String.Equals(state, DeviceSaleCompletedState) ||
                String.Equals(state, DeviceSaleFailedState) ||
                String.Equals(state, DeviceUpdatingState)))
            {
                CurrentDeviceState = state;
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    UpdateStatus();
                    await ShowSnackBarAsync($"Device {state}", true);
                });
            }
        }

        private async Task InitAndConnectDeviceAsync(string idNumber)
        {
            var res = await InitSDKAsync();
            if (res)
            {
                SelectedDevice = new PaymentDevice() { IdNumber = idNumber };
                await ConnectDeviceAsync(SelectedDevice);
            }
        }

        private void OnLogMessageChanged(string message)
        {
            if (DeviceInfo.Current.Platform == DevicePlatform.iOS &&
                CurrentDeviceState == DeviceConnectingFailedState)
            {
                CurrentDeviceState = null;
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("Warning", message, "Ok");
                });
            }

            PrevLogMessage = LatestLogMessage;
            LatestLogMessage = message;
            UpdateStatus();
        }

        private async Task GetDeviceInfoAsync()
        {
            ShowLoader("Getting Device Info...");
            var result = await _paymentSdkService.GetDeviceAsync();
            if (result.OperationSucceeded)
            {
                SelectedDevice = result.Result;
            }
            else
            {
                SelectedDevice = null;
                await Application.Current.MainPage.DisplayAlert("Warning", result.Message, "Ok");
            }
            UpdateStatus();
            HideLoader();
        }

        private async Task ConnectDeviceAsync(PaymentDevice device)
        {
            ShowLoader("Connecting to Device...");
            var token = await GetToken();
            var result = await _paymentSdkService.ConnectDeviceAsync(device.IdNumber, token);
            if (result.OperationSucceeded)
            {
                SelectedDevice = result!.Result;

                if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
                {
                    await GetDeviceInfoAsync();
                    Preferences.Default.Set("DeviceIdNumber", SelectedDevice.IdNumber);
                    IsPaired = true;
                    HideLoader();
                    CurrentDeviceState = DeviceConnectedState;
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        UpdateStatus();
                        await ShowSnackBarAsync($"Device {DeviceConnectedState}", true);
                    });
                }
            }
            else
            {
                //ReInitSdk();
                HideLoader();
                await Application.Current.MainPage.DisplayAlert("Warning", result.Message, "Ok");
            }
            UpdateStatus();
        }

        private async void ReInitSdk()
        {
            if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
            {
                ShowLoader("Re-Initialize SDKe...");
                await _paymentSdkService.ResetSdkAsync();
                var tkn = await GetToken();
                await _paymentSdkService.IntitializeSdkAsync("SBX", tkn);
                HideLoader();
                UpdateStatus();
            }
        }

        private void UpdateStatus()
        {
            IsInitialized = _paymentSdkService.IsInitialized;
            IsConnected = _paymentSdkService.IsDeviceConnected;
        }

        /// <summary>
        /// GetToken
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetToken()
        {
            var httpClient = new HttpClient();
            var body = new Body();

            if (!String.IsNullOrEmpty(_token))
            {
                return _token;
            }

            try
            {
                var authRequest = new HttpRequestMessage(HttpMethod.Post, "https://xxx.xxx.xxx.com/xxx/list?method=authenticateworker");
                ...
                var paymentTokenData = JsonConvert.DeserializeObject(response) as JObject;
                ...
                _token = paymentToken!;
                return paymentToken!;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception GetToken: {ex.Message}");
                return null;
            }

        }

        public class Body
        {
            public string CompanyID { get; set; } = "xxx";
            public string Username { get; set; } = "xxx";
            public string Password { get; set; } = "xxx";
        }

        public class Auth
        {
            public string AuthToken { get; set; }
        }

        #endregion
    }
}