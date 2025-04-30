using System;
using Com.Paysimpleandroid.Paysimpleandroidsdk.Bbpos;
using Java.Math;
using WpMAUIApp.Lib.Models;

namespace WpMAUIApp.Lib.Platforms.Android.Utils;

internal static class MixedExtensions
{
    public static PaysimpleSaleRequest ToNativeSaleParams(this MakeSaleParams makeSaleParams)
    {
        BigDecimal amountBigDecimal = new BigDecimal(makeSaleParams.Amount);

        return new PaysimpleSaleRequest()
        {
            Amount = amountBigDecimal,
            ExternalId = makeSaleParams.ExternalId ?? string.Empty,
            Description = makeSaleParams.PaymentDescription ?? "TestDescription",
        };
    }
}

