namespace WpMAUIApp.Lib.Models;

public class PaymentDevice : IDisplayable
{
    public string IdNumber { get; set; }
    public string Manufacturer { get; set; }
    public string Model { get; set; }
    public string BatteryPercentage { get; set; }

    public string DisplayTitle => $"{Manufacturer} {Model}";
    public string DisplaySubtitle => $"Id: {IdNumber}" + (string.IsNullOrEmpty(BatteryPercentage) ? string.Empty : $". Battery level: {BatteryPercentage}%");
}