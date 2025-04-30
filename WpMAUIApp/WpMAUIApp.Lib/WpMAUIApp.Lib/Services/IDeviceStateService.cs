using System;
namespace WpMAUIApp.Lib.Services
{
	public interface IDeviceStateService
	{
		Command<string> DeviceStateChanged { get; set; }
	}
}

