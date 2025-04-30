#import <Foundation/Foundation.h>
#import "PsSaleOutcome.h"
#import "PsCardData.h"
#import "PsEmvResponseData.h"

@interface PsSaleResponse : NSObject

@property (nonatomic, copy, readonly) NSString *transaction_id;
@property (nonatomic, copy, readonly) NSString *acquirer_message;
@property (nonatomic, copy, readonly) NSString *authorization_code;
@property (nonatomic, copy, readonly) NSDecimalNumber *approved_amount;
@property (nonatomic, assign, readonly) NSNumber *batch_id;
@property (nonatomic, strong, readonly) PsCardData *card;
@property (nonatomic, strong, readonly) PsSaleOutcome *outcome;
@property (nonatomic, strong, readonly) PsEmvResponseData *emv;
@property (nonatomic, readonly) NSString *entry_mode;
@property (nonatomic, readonly) NSString *processor_response;

/**
 * Initialize a new instance with the given parameters
 */
- (instancetype)initWithId:(NSString *)transaction_id
           acquirerMessage:(NSString *)acquirer_message
         authorizationCode:(NSString *)authorization_code
            approvedAmount:(NSDecimalNumber *)approved_amount
                   batchId:(NSNumber *)batch_id
                      card:(PsCardData *)card
                   outcome:(PsSaleOutcome *)outcome
                       emv:(PsEmvResponseData *)emv
                 entryMode:(NSString *)entry_mode
         processorResponse:(NSString *)processor_response;

- (instancetype)initWithDictionary:(NSDictionary *)dictionary;

- (NSDictionary *)dictionaryRepresentation;

@end
