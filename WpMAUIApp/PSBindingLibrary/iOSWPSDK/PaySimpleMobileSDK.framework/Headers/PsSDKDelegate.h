@protocol PsSDKDelegate <NSObject>

@optional
- (void)deviceConnectionDidChange:(NSString *)deviceType
             isConnected:(BOOL)isConnected;

- (void)onBbposDisplayText:(NSString *)text;

- (void)onBbposRemoveCard;

- (void)onBbposDeviceInitializationProgress:(double)currentProgress
                                description:(NSString *)description
                                      model:(NSString *)model
                               serialNumber:(NSString *)serialNumber
                                currentStep:(NSString *)currentStep;

- (void)onBbposBatteryLow;

- (void)onBbposDidError:(NSError *)error;

- (void)onBbposDidDisconnect;

- (void)onBbposDidConnect;
@end
