//
//  PsDeviceInfo.h
//  PaySimpleMobileSDK
//
//  Created by Pete on 3/5/21.
//  Copyright © 2021 Pete Yates. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface PsDeviceInfo : NSObject

@property (nonatomic, copy, readonly) NSString *device_description;
@property (nonatomic, copy, readonly) NSString *serial_number;
@property (nonatomic, copy, readonly) NSString *firmware_version;
@property (nonatomic, copy, readonly) NSString *configuration_version;
@property (nonatomic, copy, readonly) NSString *battery_percentage;
@property (nonatomic, copy, readonly) NSString *battery_level;

- (instancetype)initWithSerialNumber: (NSString *)serialNumber
                   deviceDescription: (NSString *)deviceDescription
                     firmwareVersion: (NSString *)firmwareVerion
                       configVersion: (NSString *)configVersion
                          batPercent: (NSString *)batPercent
                            batLevel: (NSString *)batLevel;



- (instancetype)initWithDictionary:(NSDictionary *)dictionary;

- (NSDictionary *)dictionaryRepresentation;

@end
