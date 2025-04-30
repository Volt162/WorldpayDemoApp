#import <Foundation/Foundation.h>

@interface PsCardData : NSObject

@property (nonatomic, copy, readonly) NSString *card_brand;
@property (nonatomic, copy, readonly) NSString *last4;
@property (nonatomic, copy, readonly) NSString *card_holder_name;
@property (nonatomic, copy, readonly) NSNumber *expiration_month;
@property (nonatomic, copy, readonly) NSNumber *expiration_year;

/**
* Initialize a new instance with the given parameters
*/
- (instancetype)initWithLast4:(NSString *)last4
                    cardBrand:(NSString *)card_brand
               cardHolderName:(NSString *)card_holder_name
              expirationMonth:(NSNumber *)expiration_month
               expirationYear:(NSNumber *)expiration_year;

- (instancetype)initWithDictionary:(NSDictionary *)dictionary;

- (NSDictionary *)dictionaryRepresentation;

@end
