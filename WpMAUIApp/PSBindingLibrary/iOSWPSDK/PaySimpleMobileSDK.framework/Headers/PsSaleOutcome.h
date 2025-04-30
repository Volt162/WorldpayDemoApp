#import <Foundation/Foundation.h>

@interface PsSaleOutcome : NSObject

@property (nonatomic, copy, readonly) NSString *result;
@property (nonatomic, copy, readonly) NSString *code;
@property (nonatomic, copy, readonly) NSString *outcome_description;

/**
 * Initialize a new instance with the given parameters
 */
- (instancetype)initWithResult:(NSString *)result
                          code:(NSString *)code
             outcomeDescription:(NSString *)outcome_description;

- (instancetype)initWithDictionary:(NSDictionary *)dictionary;

- (NSDictionary *)dictionaryRepresentation;
;

@end
