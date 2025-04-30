using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WpMAUIApp.Lib.Services;
using WpMAUIApp.ViewModels;

namespace WpMAUIApp.Pages;

public partial class MainPage : BaseContentPage
{
	public MainPage(MainPageViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
        border.SizeChanged += BorderSizeChanged;
    }

    private void BorderSizeChanged(object? sender, EventArgs e)
    {
        if (border.Height > 0)
        {
            scroll.HeightRequest = border.Height;
            border.SizeChanged -= BorderSizeChanged;
        }
    }

    void SfUraniumTextFieldTextChanged(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
        if (e.NewTextValue.Contains('.'))
        {
            MakeTwoDecimalPlaces(e, '.');
        }
        else if (e.NewTextValue.Contains(','))
        {
            MakeTwoDecimalPlaces(e, ',');
        }
    }

    private void MakeTwoDecimalPlaces(TextChangedEventArgs e, char sep)
    {
        int count = e.NewTextValue.Count(x => x == sep);
        if (count == 2)
        {
            amount.Text = amount.Text.Remove(amount.Text.Length - 1);
            amount.SelectionLength = amount.Text.Length;
            return;
        }

        if (e.NewTextValue.Contains(sep))
        {
            if (e.NewTextValue.Length - 1 - e.NewTextValue.IndexOf(sep) > 2)
            {
                var s = e.NewTextValue.Substring(0, e.NewTextValue.IndexOf(sep) + 2 + 1);
                amount.Text = s;
                amount.SelectionLength = s.Length;
                amount.CursorPosition = s.Length;
            }
        }
    }

    void AmountFocused(System.Object sender, Microsoft.Maui.Controls.FocusEventArgs e)
    {
        if (amount.Text == "0.00" || amount.Text == "0,00")
        {
            amount.Text = string.Empty;
        }
    }

    void AmountUnfocused(System.Object sender, Microsoft.Maui.Controls.FocusEventArgs e)
    {
        if (amount.Text == string.Empty)
        {
            amount.Text = "0.00";
        }
    }
}
