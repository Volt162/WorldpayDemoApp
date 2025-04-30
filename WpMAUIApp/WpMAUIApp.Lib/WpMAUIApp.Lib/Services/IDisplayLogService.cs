using System;
namespace WpMAUIApp.Lib.Services
{
	public interface IDisplayLogService
	{
        Command<string> LogMessageChanged { get; set; }
    }
}

