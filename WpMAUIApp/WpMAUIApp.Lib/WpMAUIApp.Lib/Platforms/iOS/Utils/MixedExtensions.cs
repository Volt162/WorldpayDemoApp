using System.Globalization;
using Foundation;
using PsMobileSDKBindingsLib;
using WpMAUIApp.Lib.Models;

namespace WpMAUIApp.Lib.Platforms.iOS.Utils;

internal static class MixedExtensions
{
    public static PsSaleRequest ToNativeSaleParams(this MakeSaleParams makeSaleParams)
    {
        NSDecimalNumber amount = new NSDecimalNumber(makeSaleParams!.Amount);

        return new PsSaleRequest(
            amount,
            makeSaleParams.DuplicateCheck ?? string.Empty,
            makeSaleParams.Currency ?? string.Empty,
            makeSaleParams.ExternalId ?? string.Empty,
            makeSaleParams.PaymentDescription ?? "payment",
            makeSaleParams.Capture ?? string.Empty,
            makeSaleParams!.AllowPartialApprovals
        );
    }

    public static decimal ToDecimal(this NSDecimalNumber number)
    {
        var stringRepresentation = number.ToString();
        return decimal.Parse(stringRepresentation, CultureInfo.InvariantCulture);
    }
}

