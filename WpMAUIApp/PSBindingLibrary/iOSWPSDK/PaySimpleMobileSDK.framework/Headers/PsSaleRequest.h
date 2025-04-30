#import <Foundation/Foundation.h>

@interface PsSaleRequest : NSObject

@property (nonatomic, copy, readonly) NSDecimalNumber *amount;
@property (nonatomic, copy, readonly) NSString *duplicate_check;
@property (nonatomic, copy, readonly) NSString *currency;
@property (nonatomic, copy, readonly) NSString *external_id;
@property (nonatomic, copy, readonly) NSString *card_sale_description;
@property (nonatomic, copy, readonly) NSString *capture;
@property (nonatomic, assign, readonly) BOOL allow_partial_approvals;

/**
 * Initialize a new instance with the given parameters
 */
- (instancetype)initWithamount:(NSDecimalNumber *)amount
                duplicateCheck:(NSString *)duplicate_check
                      currency:(NSString *)currency
                    externalId:(NSString *)external_id
           cardSaleDescription:(NSString *)card_sale_description
                       capture:(NSString *)capture
         allowPartialApprovals:(BOOL)allow_partial_approvals;

- (NSDictionary *)dictionaryRepresentation;

@end
