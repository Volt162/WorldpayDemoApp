using System;
using Foundation;
using ObjCRuntime;

namespace PsMobileSDKBindingsLib
{
    // @interface PsSaleOutcome : NSObject
    [BaseType(typeof(NSObject))]
    interface PsSaleOutcome
    {
        // @property (readonly, copy, nonatomic) NSString * result;
        [Export("result")]
        string Result { get; }

        // @property (readonly, copy, nonatomic) NSString * code;
        [Export("code")]
        string Code { get; }

        // @property (readonly, copy, nonatomic) NSString * outcome_description;
        [Export("outcome_description")]
        string Outcome_description { get; }

        // -(instancetype)initWithResult:(NSString *)result code:(NSString *)code outcomeDescription:(NSString *)outcome_description;
        [Export("initWithResult:code:outcomeDescription:")]
        NativeHandle Constructor(string result, string code, string outcome_description);

        // -(instancetype)initWithDictionary:(NSDictionary *)dictionary;
        [Export("initWithDictionary:")]
        NativeHandle Constructor(NSDictionary dictionary);

        // -(NSDictionary *)dictionaryRepresentation;
        [Export("dictionaryRepresentation")]
        NSDictionary DictionaryRepresentation { get; }
    }

    // @interface PsCardData : NSObject
    [BaseType(typeof(NSObject))]
    interface PsCardData
    {
        // @property (readonly, copy, nonatomic) NSString * card_brand;
        [Export("card_brand")]
        string Card_brand { get; }

        // @property (readonly, copy, nonatomic) NSString * last4;
        [Export("last4")]
        string Last4 { get; }

        // @property (readonly, copy, nonatomic) NSString * card_holder_name;
        [Export("card_holder_name")]
        string Card_holder_name { get; }

        // @property (readonly, copy, nonatomic) NSNumber * expiration_month;
        [Export("expiration_month", ArgumentSemantic.Copy)]
        NSNumber Expiration_month { get; }

        // @property (readonly, copy, nonatomic) NSNumber * expiration_year;
        [Export("expiration_year", ArgumentSemantic.Copy)]
        NSNumber Expiration_year { get; }

        // -(instancetype)initWithLast4:(NSString *)last4 cardBrand:(NSString *)card_brand cardHolderName:(NSString *)card_holder_name expirationMonth:(NSNumber *)expiration_month expirationYear:(NSNumber *)expiration_year;
        [Export("initWithLast4:cardBrand:cardHolderName:expirationMonth:expirationYear:")]
        NativeHandle Constructor(string last4, string card_brand, string card_holder_name, NSNumber expiration_month, NSNumber expiration_year);

        // -(instancetype)initWithDictionary:(NSDictionary *)dictionary;
        [Export("initWithDictionary:")]
        NativeHandle Constructor(NSDictionary dictionary);

        // -(NSDictionary *)dictionaryRepresentation;
        [Export("dictionaryRepresentation")]
        NSDictionary DictionaryRepresentation { get; }
    }

    // @interface PsEmvResponseData : NSObject
    [BaseType(typeof(NSObject))]
    interface PsEmvResponseData
    {
        // @property (readonly, copy, nonatomic) NSString * application_identifier;
        [Export("application_identifier")]
        string Application_identifier { get; }

        // @property (readonly, copy, nonatomic) NSString * application_preferred_name;
        [Export("application_preferred_name")]
        string Application_preferred_name { get; }

        // @property (readonly, copy, nonatomic) NSString * application_label;
        [Export("application_label")]
        string Application_label { get; }

        // @property (readonly, copy, nonatomic) NSString * cryptogram;
        [Export("cryptogram")]
        string Cryptogram { get; }

        // @property (readonly, copy, nonatomic) NSString * host_response_code;
        [Export("host_response_code")]
        string Host_response_code { get; }

        // @property (readonly, copy, nonatomic) NSString * host_response_message;
        [Export("host_response_message")]
        string Host_response_message { get; }

        // -(instancetype)initWithApplicationIdentifier:(NSString *)application_identifier applicationPreferredName:(NSString *)application_preferred_name applicationLabel:(NSString *)application_label cryptogram:(NSString *)cryptogram hostResponseCode:(NSString *)host_response_code hostResponseMessage:(NSString *)host_response_message;
        [Export("initWithApplicationIdentifier:applicationPreferredName:applicationLabel:cryptogram:hostResponseCode:hostResponseMessage:")]
        NativeHandle Constructor(string application_identifier, string application_preferred_name, string application_label, string cryptogram, string host_response_code, string host_response_message);

        // -(instancetype)initWithDictionary:(NSDictionary *)dictionary;
        [Export("initWithDictionary:")]
        NativeHandle Constructor(NSDictionary dictionary);

        // -(NSDictionary *)dictionaryRepresentation;
        [Export("dictionaryRepresentation")]
        NSDictionary DictionaryRepresentation { get; }
    }

    // @interface PsSaleResponse : NSObject
    [BaseType(typeof(NSObject))]
    interface PsSaleResponse
    {
        // @property (readonly, copy, nonatomic) NSString * transaction_id;
        [Export("transaction_id")]
        string Transaction_id { get; }

        // @property (readonly, copy, nonatomic) NSString * acquirer_message;
        [Export("acquirer_message")]
        string Acquirer_message { get; }

        // @property (readonly, copy, nonatomic) NSString * authorization_code;
        [Export("authorization_code")]
        string Authorization_code { get; }

        // @property (readonly, copy, nonatomic) NSDecimalNumber * approved_amount;
        [Export("approved_amount", ArgumentSemantic.Copy)]
        NSDecimalNumber Approved_amount { get; }

        // @property (readonly, assign, nonatomic) NSNumber * batch_id;
        [Export("batch_id", ArgumentSemantic.Assign)]
        NSNumber Batch_id { get; }

        // @property (readonly, nonatomic, strong) PsCardData * card;
        [Export("card", ArgumentSemantic.Strong)]
        PsCardData Card { get; }

        // @property (readonly, nonatomic, strong) PsSaleOutcome * outcome;
        [Export("outcome", ArgumentSemantic.Strong)]
        PsSaleOutcome Outcome { get; }

        // @property (readonly, nonatomic, strong) PsEmvResponseData * emv;
        [Export("emv", ArgumentSemantic.Strong)]
        PsEmvResponseData Emv { get; }

        // @property (readonly, nonatomic) NSString * entry_mode;
        [Export("entry_mode")]
        string Entry_mode { get; }

        // @property (readonly, nonatomic) NSString * processor_response;
        [Export("processor_response")]
        string Processor_response { get; }

        // -(instancetype)initWithId:(NSString *)transaction_id acquirerMessage:(NSString *)acquirer_message authorizationCode:(NSString *)authorization_code approvedAmount:(NSDecimalNumber *)approved_amount batchId:(NSNumber *)batch_id card:(PsCardData *)card outcome:(PsSaleOutcome *)outcome emv:(PsEmvResponseData *)emv entryMode:(NSString *)entry_mode processorResponse:(NSString *)processor_response;
        [Export("initWithId:acquirerMessage:authorizationCode:approvedAmount:batchId:card:outcome:emv:entryMode:processorResponse:")]
        NativeHandle Constructor(string transaction_id, string acquirer_message, string authorization_code, NSDecimalNumber approved_amount, NSNumber batch_id, PsCardData card, PsSaleOutcome outcome, PsEmvResponseData emv, string entry_mode, string processor_response);

        // -(instancetype)initWithDictionary:(NSDictionary *)dictionary;
        [Export("initWithDictionary:")]
        NativeHandle Constructor(NSDictionary dictionary);

        // -(NSDictionary *)dictionaryRepresentation;
        [Export("dictionaryRepresentation")]
        NSDictionary DictionaryRepresentation { get; }
    }

    // @interface PsSaleRequest : NSObject
    [BaseType(typeof(NSObject))]
    interface PsSaleRequest
    {
        // @property (readonly, copy, nonatomic) NSDecimalNumber * amount;
        [Export("amount", ArgumentSemantic.Copy)]
        NSDecimalNumber Amount { get; }

        // @property (readonly, copy, nonatomic) NSString * duplicate_check;
        [Export("duplicate_check")]
        string Duplicate_check { get; }

        // @property (readonly, copy, nonatomic) NSString * currency;
        [Export("currency")]
        string Currency { get; }

        // @property (readonly, copy, nonatomic) NSString * external_id;
        [Export("external_id")]
        string External_id { get; }

        // @property (readonly, copy, nonatomic) NSString * card_sale_description;
        [Export("card_sale_description")]
        string Card_sale_description { get; }

        // @property (readonly, copy, nonatomic) NSString * capture;
        [Export("capture")]
        string Capture { get; }

        // @property (readonly, assign, nonatomic) BOOL allow_partial_approvals;
        [Export("allow_partial_approvals")]
        bool Allow_partial_approvals { get; }

        // -(instancetype)initWithamount:(NSDecimalNumber *)amount duplicateCheck:(NSString *)duplicate_check currency:(NSString *)currency externalId:(NSString *)external_id cardSaleDescription:(NSString *)card_sale_description capture:(NSString *)capture allowPartialApprovals:(BOOL)allow_partial_approvals;
        [Export("initWithamount:duplicateCheck:currency:externalId:cardSaleDescription:capture:allowPartialApprovals:")]
        NativeHandle Constructor(NSDecimalNumber amount, string duplicate_check, string currency, string external_id, string card_sale_description, string capture, bool allow_partial_approvals);

        // -(NSDictionary *)dictionaryRepresentation;
        [Export("dictionaryRepresentation")]
        NSDictionary DictionaryRepresentation { get; }
    }

    // @protocol PsSDKDelegate <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface PsSDKDelegate
    {
        // @optional -(void)deviceConnectionDidChange:(NSString *)deviceType isConnected:(BOOL)isConnected;
        [Export("deviceConnectionDidChange:isConnected:")]
        void DeviceConnectionDidChange(string deviceType, bool isConnected);

        // @optional -(void)onBbposDisplayText:(NSString *)text;
        [Export("onBbposDisplayText:")]
        void OnBbposDisplayText(string text);

        // @optional -(void)onBbposRemoveCard;
        [Export("onBbposRemoveCard")]
        void OnBbposRemoveCard();

        // @optional -(void)onBbposDeviceInitializationProgress:(double)currentProgress description:(NSString *)description model:(NSString *)model serialNumber:(NSString *)serialNumber currentStep:(NSString *)currentStep;
        [Export("onBbposDeviceInitializationProgress:description:model:serialNumber:currentStep:")]
        void OnBbposDeviceInitializationProgress(double currentProgress, string description, string model, string serialNumber, string currentStep);

        // @optional -(void)onBbposBatteryLow;
        [Export("onBbposBatteryLow")]
        void OnBbposBatteryLow();

        // @optional -(void)onBbposDidError:(NSError *)error;
        [Export("onBbposDidError:")]
        void OnBbposDidError(NSError error);

        // @optional -(void)onBbposDidDisconnect;
        [Export("onBbposDidDisconnect")]
        void OnBbposDidDisconnect();

        // @optional -(void)onBbposDidConnect;
        [Export("onBbposDidConnect")]
        void OnBbposDidConnect();
    }

    // @interface PsDeviceInfo : NSObject
    [BaseType(typeof(NSObject))]
    interface PsDeviceInfo
    {
        // @property (readonly, copy, nonatomic) NSString * device_description;
        [Export("device_description")]
        string Device_description { get; }

        // @property (readonly, copy, nonatomic) NSString * serial_number;
        [Export("serial_number")]
        string Serial_number { get; }

        // @property (readonly, copy, nonatomic) NSString * firmware_version;
        [Export("firmware_version")]
        string Firmware_version { get; }

        // @property (readonly, copy, nonatomic) NSString * configuration_version;
        [Export("configuration_version")]
        string Configuration_version { get; }

        // @property (readonly, copy, nonatomic) NSString * battery_percentage;
        [Export("battery_percentage")]
        string Battery_percentage { get; }

        // @property (readonly, copy, nonatomic) NSString * battery_level;
        [Export("battery_level")]
        string Battery_level { get; }

        // -(instancetype)initWithSerialNumber:(NSString *)serialNumber deviceDescription:(NSString *)deviceDescription firmwareVersion:(NSString *)firmwareVerion configVersion:(NSString *)configVersion batPercent:(NSString *)batPercent batLevel:(NSString *)batLevel;
        [Export("initWithSerialNumber:deviceDescription:firmwareVersion:configVersion:batPercent:batLevel:")]
        NativeHandle Constructor(string serialNumber, string deviceDescription, string firmwareVerion, string configVersion, string batPercent, string batLevel);

        // -(instancetype)initWithDictionary:(NSDictionary *)dictionary;
        [Export("initWithDictionary:")]
        NativeHandle Constructor(NSDictionary dictionary);

        // -(NSDictionary *)dictionaryRepresentation;
        [Export("dictionaryRepresentation")]
        NSDictionary DictionaryRepresentation { get; }
    }

    // @interface PsBluetoothDevice : NSObject
    [BaseType(typeof(NSObject))]
    interface PsBluetoothDevice
    {
        // @property (readonly, copy, nonatomic) NSString * serial_number;
        [Export("serial_number")]
        string Serial_number { get; }

        // @property (readonly, copy, nonatomic) NSString * manufacturer;
        [Export("manufacturer")]
        string Manufacturer { get; }

        // @property (readonly, copy, nonatomic) NSString * model;
        [Export("model")]
        string Model { get; }

        // -(instancetype)initWithSerialNumber:(NSString *)serialNumber manufacturer:(NSString *)manufacturer model:(NSString *)model;
        [Export("initWithSerialNumber:manufacturer:model:")]
        NativeHandle Constructor(string serialNumber, string manufacturer, string model);

        // -(instancetype)initWithDictionary:(NSDictionary *)dictionary;
        [Export("initWithDictionary:")]
        NativeHandle Constructor(NSDictionary dictionary);

        // -(NSDictionary *)dictionaryRepresentation;
        [Export("dictionaryRepresentation")]
        NSDictionary DictionaryRepresentation { get; }
    }

    // @interface PsSDK : NSObject
    [BaseType(typeof(NSObject))]
    interface PsSDK
    {
        [Wrap("WeakDelegate")]
        PsSDKDelegate Delegate { get; set; }

        // @property (nonatomic, weak) id<PsSDKDelegate> delegate;
        [NullAllowed, Export("delegate", ArgumentSemantic.Weak)]
        NSObject WeakDelegate { get; set; }

        // +(BOOL)initializeSdk:(PsSDKDeviceType)device environment:(NSString *)env error:(NSError **)error;
        [Static]
        [Export("initializeSdk:environment:error:")]
        bool InitializeSdk(nuint device, string env, out NSError error);

        // -(void)resetSdk;
        [Export("resetSdk")]
        void ResetSdk();

        // +(PsSDK *)sharedInstance;
        [Static]
        [Export("sharedInstance")]
        PsSDK SharedInstance { get; }

        // +(BOOL)isInitialized;
        [Export("isInitialized")]
        bool IsInitialized { get; }

        // -(void)disconnectDeviceWithHandler:(void (^)(BOOL, NSError *))handler;
        [Export("disconnectDeviceWithHandler:")]
        [Async]
        void DisconnectDeviceWithHandler(Action<bool, NSError> handler);

        // -(void)makeSaleWithClientToken:(NSString *)clientToken request:(PsSaleRequest *)saleRequest completionHandler:(void (^)(PsSaleResponse *, NSError *))handler;
        [Export("makeSaleWithClientToken:request:completionHandler:")]
        [Async]
        void MakeSaleWithClientToken(string clientToken, PsSaleRequest saleRequest, Action<PsSaleResponse, NSError> handler);

        // -(void)psInternalSaleWithReferenceNumber:(NSString *)refNumber ticketNumber:(NSString *)ticketNumber request:(PsSaleRequest *)saleRequest completionHandler:(void (^)(PsSaleResponse *, NSError *))handler;
        [Export("psInternalSaleWithReferenceNumber:ticketNumber:request:completionHandler:")]
        [Async]
        void PsInternalSaleWithReferenceNumber(string refNumber, string ticketNumber, PsSaleRequest saleRequest, Action<PsSaleResponse, NSError> handler);

        // -(void)psInternalAuthWithReferenceNumber:(NSString *)refNumber ticketNumber:(NSString *)ticketNumber request:(PsSaleRequest *)saleRequest completionHandler:(void (^)(PsSaleResponse *, NSError *))handler;
        [Export("psInternalAuthWithReferenceNumber:ticketNumber:request:completionHandler:")]
        [Async]
        void PsInternalAuthWithReferenceNumber(string refNumber, string ticketNumber, PsSaleRequest saleRequest, Action<PsSaleResponse, NSError> handler);

        // -(BOOL)isDeviceConnected;
        [Export("isDeviceConnected")]
        bool IsDeviceConnected { get; }

        // -(void)scanForBluetoothDevicesWithClientToken:(NSString *)clientToken completionHandler:(void (^)(NSArray<PsBluetoothDevice *> *, NSError *))handler;
        [Export("scanForBluetoothDevicesWithClientToken:completionHandler:")]
        [Async]
        void ScanForBluetoothDevicesWithClientToken(string clientToken, Action<PsBluetoothDevice[], NSError> handler);

        // -(void)connectDeviceWithClientToken:(NSString *)clientToken serialNumber:(NSString *)serialNumber completionHandler:(void (^)(BOOL, PsDeviceInfo *, NSError *))handler;
        [Export("connectDeviceWithClientToken:serialNumber:completionHandler:")]
        [Async(ResultTypeName = "ConnectDeviceWithClientTokenResult")]
        void ConnectDeviceWithClientToken(string clientToken, string serialNumber, Action<bool, PsDeviceInfo, NSError> handler);

        // -(void)connectDeviceWithHandler:(void (^)(BOOL, PsDeviceInfo *, NSError *))handler;
        [Export("connectDeviceWithHandler:")]
        [Async(ResultTypeName = "ConnectDeviceResult")]
        void ConnectDeviceWithHandler(Action<bool, PsDeviceInfo, NSError> handler);

        // -(NSString *)getDeviceName;
        [Export("getDeviceName")]
        string DeviceName { get; }

        // -(NSString *)getDeviceBatteryPercentage;
        [Export("getDeviceBatteryPercentage")]
        string DeviceBatteryPercentage { get; }

        // -(NSString *)getDeviceSerialNumber;
        [Export("getDeviceSerialNumber")]
        string DeviceSerialNumber { get; }

        // -(void)stopCurrentBbposFlow;
        [Export("stopCurrentBbposFlow")]
        void StopCurrentBbposFlow();
    }
}
