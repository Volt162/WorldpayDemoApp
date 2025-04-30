#import <Foundation/Foundation.h>
#import "PsSaleResponse.h"
#import "PsSaleRequest.h"
#import "PsSDKDelegate.h"
#import "PsDeviceInfo.h"
#import "PsBluetoothDevice.h"

enum
{
    IDYNAMO, // iOS only
    BBPOS
};

typedef NSUInteger PsSDKDeviceType;

@interface PsSDK : NSObject

@property (nonatomic, weak) id<PsSDKDelegate> delegate;

+ (BOOL)initializeSdk:(PsSDKDeviceType)device
             environment:(NSString *)env
                   error:(NSError **)error;

- (void) resetSdk;

#pragma mark - Singleton access

+ (PsSDK *)sharedInstance;

+ (BOOL) isInitialized;

- (void) disconnectDeviceWithHandler: (void(^)(BOOL didDisconnect, NSError *error)) handler;

- (void) makeSaleWithClientToken:(NSString *)clientToken
                         request:(PsSaleRequest *)saleRequest
               completionHandler:(void(^)(PsSaleResponse *response, NSError *error))handler;

- (void) psInternalSaleWithReferenceNumber:(NSString *)refNumber
                              ticketNumber:(NSString *)ticketNumber
                                   request:(PsSaleRequest *)saleRequest
                         completionHandler:(void(^)(PsSaleResponse *response, NSError *error))handler;

- (void) psInternalAuthWithReferenceNumber:(NSString *)refNumber
                              ticketNumber:(NSString *)ticketNumber
                                   request:(PsSaleRequest *)saleRequest
                         completionHandler:(void(^)(PsSaleResponse *response, NSError *error))handler;


- (BOOL) isDeviceConnected;

// Connecting to BBPOS device
- (void) scanForBluetoothDevicesWithClientToken: (NSString *)clientToken
                              completionHandler: (void(^)(NSArray<PsBluetoothDevice *> *, NSError *error))handler;

- (void) connectDeviceWithClientToken:(NSString *)clientToken
                         serialNumber:(NSString *)serialNumber
                    completionHandler: (void(^)(BOOL didConnect, PsDeviceInfo *deviceInfo, NSError *error))handler;

// Connecting to device that does not require a client token
- (void) connectDeviceWithHandler: (void(^)(BOOL didConnect, PsDeviceInfo *deviceInfo, NSError *error))handler;

- (NSString *) getDeviceName;

- (NSString *) getDeviceBatteryPercentage;

- (NSString *) getDeviceSerialNumber;

- (void) stopCurrentBbposFlow;
@end
