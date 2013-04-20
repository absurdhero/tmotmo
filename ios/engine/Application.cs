using System;

#if MonoTouch
using MonoTouch.Foundation;
using MonoTouch.UIKit;
#endif

/// Functions that interact with the application's environment
public class Application
{
    public static void OpenURL(string url)
    {
        #if MonoTouch
        UIApplication.SharedApplication.OpenUrl(new NSUrl(url));
        #else
        // do nothing on other builds
        #endif
    }
}


