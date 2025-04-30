using System;
using System.Diagnostics;
using Foundation;
using PsMobileSDKBindingsLib;

namespace WpMAUIApp.Lib.Platforms.iOS;

public class PsDelegate : PsSDKDelegate
{
    private readonly Command<string> _stateCommand;
    private readonly Command<string> _logCommand;
    private readonly Command<string> _displayTextCommand;

    public PsDelegate(Command<string> stateCommand, Command<string> logCommand, Command<string> displayTextCommand)
	{
        _stateCommand = stateCommand;
        _logCommand = logCommand;
        _displayTextCommand = displayTextCommand;
    }

    public override void OnBbposBatteryLow()
    {
        _stateCommand?.Execute("BatteryLow");
        AppLogger("BbposBatteryLow");
    }

    public override void OnBbposDeviceInitializationProgress(double currentProgress, string description, string model, string serialNumber, string currentStep)
    {
        _stateCommand?.Execute("Updating");
        AppLogger($"BbposDeviceInitializationProgress currentProgress={currentProgress}, description={description}, model={model}, serialNumber={serialNumber}, currentStep={currentStep}");
    }

    public override void OnBbposDidConnect()
    {
        _stateCommand?.Execute("Connected");
        AppLogger("BbposDidConnect");
    }

    public override void OnBbposDidDisconnect()
    {
        _stateCommand?.Execute("Disconnected");
        AppLogger("BbposDidDisconnect");
    }

    public override void OnBbposDisplayText(string text)
    {
        _displayTextCommand?.Execute(text);
        AppLogger($"BbposDisplayText text={text}");
    }

    public override void OnBbposDidError(NSError error)
    {
        if (String.Equals(error?.LocalizedDescription, "Timeout") ||
            String.Equals(error?.LocalizedDescription, "Peer removed pairing information") ||
            String.Equals(error?.LocalizedDescription, "Pairing Error (Code=5800)") ||
            String.Equals(error?.LocalizedDescription, "BT connection timeout (02)"))
        {
            _stateCommand?.Execute("ConnectingFailed");
        }
        AppLogger($"BbposDidError error = {error.LocalizedDescription}");
    }

    public override void OnBbposRemoveCard()
    {
        AppLogger("BbposRemoveCard");
    }

    private void AppLogger(string mess)
    {
        Debug.WriteLine(mess);
        _logCommand?.Execute(mess);
    }
}

