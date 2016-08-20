//
//  ViewController.m
//  
//
//  Created by Superdebv on 26/05/16.
//
//

@implementation ViewController : UIViewController

-(void) shareMethod: (const char * []) paths Message : (const char *) shareMessage Subject : (const char *) shareSubject NumPath : (const int) numArray Exclude : (const char * []) excludeActivities NumExclude : (const int) numExcludeArray {

    NSMutableArray *results = [NSMutableArray array]; 
    for (int i = 0; i < numArray; i++) 
    { 
        NSString *str = [[NSString alloc] initWithCString:paths[i] encoding:NSUTF8StringEncoding];
        [results addObject:str]; 
         
    }

    NSString *message = [NSString stringWithUTF8String:shareMessage];
    NSString *title   = [NSString stringWithUTF8String:shareSubject];

    NSMutableArray * mail = [NSMutableArray array];
    [mail addObject:message];
    for (NSString *path in results) 
    {
          NSData *image = [NSData dataWithContentsOfFile:path];
         [mail addObject:image]; 
    }
    
    NSArray* arryOfImgs = [NSArray arrayWithArray:mail];

    UIActivityViewController* activityVc =
    [[UIActivityViewController alloc] initWithActivityItems:arryOfImgs
                                      applicationActivities:nil];
    
    [activityVc setValue:title forKey:@"subject"];
    

    
    NSMutableArray *excludeActivitiesResult = [NSMutableArray array]; 
    for (int i = 0; i < numExcludeArray; i++) 
    { 
        NSString *str = [[NSString alloc] initWithCString:excludeActivities[i] encoding:NSUTF8StringEncoding];
        
        if ([str isEqualToString:@"UIActivityTypeMail"])
            [excludeActivitiesResult addObject:UIActivityTypeMail];
        if ([str isEqualToString:@"UIActivityTypeMessage"])
            [excludeActivitiesResult addObject:UIActivityTypeMessage];
        if ([str isEqualToString:@"UIActivityTypePostToFacebook"])
            [excludeActivitiesResult addObject:UIActivityTypePostToFacebook];
        if ([str isEqualToString:@"UIActivityTypePostToFlickr"])
            [excludeActivitiesResult addObject:UIActivityTypePostToFlickr];
        if ([str isEqualToString:@"UIActivityTypePostToTencentWeibo"])
            [excludeActivitiesResult addObject:UIActivityTypePostToTencentWeibo];
        if ([str isEqualToString:@"UIActivityTypePostToTwitter"])
            [excludeActivitiesResult addObject:UIActivityTypePostToTwitter];
        if ([str isEqualToString:@"UIActivityTypePostToVimeo"])
            [excludeActivitiesResult addObject:UIActivityTypePostToVimeo];
        if ([str isEqualToString:@"UIActivityTypePostToWeibo"])
            [excludeActivitiesResult addObject:UIActivityTypePostToWeibo];
        if ([str isEqualToString:@"UIActivityTypePrint"])
            [excludeActivitiesResult addObject:UIActivityTypePrint];
        if ([str isEqualToString:@"UIActivityTypeSaveToCameraRoll"])
            [excludeActivitiesResult addObject:UIActivityTypeSaveToCameraRoll];
        if ([str isEqualToString:@"UIActivityTypeCopyToPasteboard"])
            [excludeActivitiesResult addObject:UIActivityTypeCopyToPasteboard];
        if ([str isEqualToString:@"UIActivityTypeAssignToContact"])
            [excludeActivitiesResult addObject:UIActivityTypeAssignToContact];
        if ([str isEqualToString:@"UIActivityTypeAirDrop"])
            [excludeActivitiesResult addObject:UIActivityTypeAirDrop];
        if ([str isEqualToString:@"UIActivityTypeAddToReadingList"])
            [excludeActivitiesResult addObject:UIActivityTypeAddToReadingList];
         
    }
 
    activityVc.excludedActivityTypes = excludeActivitiesResult;

    if ( UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad && [activityVc respondsToSelector:@selector(popoverPresentationController)] ) {
        
        UIPopoverController *popup = [[UIPopoverController alloc] initWithContentViewController:activityVc];
        
        [popup presentPopoverFromRect:CGRectMake(self.view.frame.size.width/2, self.view.frame.size.height/4, 0, 0)
                               inView:[UIApplication sharedApplication].keyWindow.rootViewController.view permittedArrowDirections:UIPopoverArrowDirectionAny animated:YES];
    }
    else
        [[UIApplication sharedApplication].keyWindow.rootViewController presentViewController:activityVc animated:YES completion:nil];
    [activityVc release];
}

@end



extern "C"{
    void _TAG_Share(const char * paths[], const char * message, const char * subject, const int numArray, const char * excludeActivities[], const int numExcludeArray){
        ViewController *vc = [[ViewController alloc] init];
        [vc shareMethod:paths Message:message Subject:subject NumPath:numArray Exclude:excludeActivities NumExclude:numExcludeArray ];
        [vc release];
    }
}

