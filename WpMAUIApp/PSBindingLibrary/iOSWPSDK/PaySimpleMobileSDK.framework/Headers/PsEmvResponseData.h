#import <Foundation/Foundation.h>

@interface PsEmvResponseData : NSObject

@property (nonatomic, copy, readonly) NSString *application_identifier;
@property (nonatomic, copy, readonly) NSString *application_preferred_name;
@property (nonatomic, copy, readonly) NSString *application_label;
@property (nonatomic, copy, readonly) NSString *cryptogram;
@property (nonatomic, copy, readonly) NSString *host_response_code;
@property (nonatomic, copy, readonly) NSString *host_response_message;

/**
* Initialize a new instance with the given parameters
*/
- (instancetype)initWithApplicationIdentifier:(NSString *)application_identifier
                     applicationPreferredName:(NSString *)application_preferred_name
                             applicationLabel:(NSString *)application_label
                                   cryptogram:(NSString *)cryptogram
                             hostResponseCode:(NSString *)host_response_code
                          hostResponseMessage:(NSString *)host_response_message;

- (instancetype)initWithDictionary:(NSDictionary *)dictionary;

- (NSDictionary *)dictionaryRepresentation;

@end
