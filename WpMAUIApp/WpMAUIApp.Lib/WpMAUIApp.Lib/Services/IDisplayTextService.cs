using System;
namespace WpMAUIApp.Lib.Services;

public interface IDisplayTextService
{
    Command<string> DisplayTextChanged { get; set; }
}

