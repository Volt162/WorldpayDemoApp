namespace WpMAUIApp.Lib.Models;

public class MakeSaleResponse : BaseSaleResponse
{
    public string AcquirerMessage { get; set; }
    public string ApprovedAmount { get; set; }
    public string AuthorizationCode { get; set; }
    public string BatchId { get; set; }
    public SaleCardData Card { get; set; }
    public EmvData Evm { get; set; }
    public string EntryMode { get; set; }
    public SaleOutcome Outcome { get; set; }
    public string TransactionId { get; set; }
}

public class BaseSaleResponse
{
    public string Description { get; set; }
    public string ECPNetworkResponse { get; set; }
    public int? HttpStatusCode { get; set; }
}

public class SaleCardData
{
    public string CardBrand { get; set; }
    public string CardHolderName { get; set; }
    public string ExpirationMonth { get; set; }
    public string ExpirationYear { get; set; }
    public string Last4 { get; set; }
}

public class EmvData
{
    public string ApplicationCryptogram { get; set; }
    public string ApplicationIdentifier { get; set; }
    public string ApplicationLabel { get; set; }
    public string ApplicationPreferredName { get; set; }
    public string AuthorizationResponseCode { get; set; }
    public string AuthorizationResponseMessage { get; set; }
    public bool IsPinVerified { get; set; }
}

public class SaleOutcome
{
    public string Code { get; set; }
    public string OutcomeDescription { get; set; }
    public string Result { get; set; }
}

