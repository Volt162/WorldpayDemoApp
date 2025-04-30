//
//  PsBluetoothDevice.h
//  PaySimpleMobileSDK
//
//  Created by Pete on 3/16/21.
//  Copyright © 2021 Pete Yates. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface PsBluetoothDevice : NSObject

@property (nonatomic, copy, readonly) NSString *serial_number;
@property (nonatomic, copy, readonly) NSString *manufacturer;
@property (nonatomic, copy, readonly) NSString *model;

- (instancetype)initWithSerialNumber: (NSString *)serialNumber
                        manufacturer: (NSString *)manufacturer
                               model: (NSString *)model;


- (instancetype)initWithDictionary:(NSDictionary *)dictionary;

- (NSDictionary *)dictionaryRepresentation;

@end
