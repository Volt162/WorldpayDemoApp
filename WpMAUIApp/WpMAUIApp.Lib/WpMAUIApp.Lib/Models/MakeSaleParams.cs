namespace WpMAUIApp.Lib.Models;

public class MakeSaleParams
{
    public MakeSaleParams(
        bool allowPartialApprovals,
        string amount,
        string capture = null,
        string currency = null,
        string duplicateCheck = null,
        string externalId = null,
        string paymentDescription = null,
        string paymentDescriptionSuffix = null)
    {
        AllowPartialApprovals = allowPartialApprovals;
        Amount = amount;
        Capture = capture;
        Currency = currency;
        DuplicateCheck = duplicateCheck;
        ExternalId = externalId;
        PaymentDescription = paymentDescription;
        PaymentDescriptionSuffix = paymentDescriptionSuffix;
    }

    public bool AllowPartialApprovals { get; set; }
    public string Amount { get; set; }
    public string Capture { get; set; }
    public string Currency { get; set; }
    public string DuplicateCheck { get; set; }
    public string ExternalId { get; set; }
    public string PaymentDescription { get; set; }
    public string PaymentDescriptionSuffix { get; set; }
}

