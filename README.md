# Secure Storage Plugin for Xamarin and Windows Apps (iPhone, Android, UWP, Mac, Tizen)
The plugin can be used to **Securely store** sensitive data strings such as password, session token, credit card number etc. This plugin securely stores the strings across sessions on iOS, Android, UWP, Mac and Tizen apps. Ver 2.0.0 is now compatible with **.net standard**.

The supported platforms include:

* iOS Unified
* Android
* UWP
* MacOS (OSX)
* Tizen

It has easy to use API of key-value pairs. The pairs are saved using platform specific encryption mechanism. It has no dependencies on any packages (including Xamarin.Forms, MVVMCross.). And can be used by any Xamarin or UWP or Tizen app. It is open source.

**Note**

If your app fails to save values in iOS 10 Simulator, open the Entitlements.plist file and make sure that "Enable Keychain Access Groups" is checked. Also ensure that in Project->Options->iOS Bundle Signing, the Entitlements.plist is selected in Custom Entitlements for iPhoneSimulator platform.
This happens only in iOS 10 Simulator due to https://forums.xamarin.com/discussion/77760/ios-10-keychain

## Usage ##
It provides 4 methods.

**SetValue** - Stores the key and value.

``` 
CrossSecureStorage.Current.SetValue(“SessionToken”, “1234567890”);
```

**GetValue** - Returns the value for the given key. If not found, returns default value.

``` 
var sessionToken = CrossSecureStorage.Current.GetValue (“SessionToken”);
``` 


**DeleteKey** - Deletes the given key and corresponding value from the storage.

``` 
CrossSecureStorage.Current.DeleteKey(“SessionToken”);
``` 

**HasKey** - Checks if the given key exists in the storage.

```
var exists = CrossSecureStorage.Current.HasKey (“SessionToken”);
``` 

In the UWP Apps, the data is stored in the password vault. It has a built in limit of 10 values per app.

The plugin can be found here:

https://www.nuget.org/packages/sameerIOTApps.Plugin.SecureStorage/

The sample apps on GitHub show how to use it in Xamarin and in Tizen Apps.

Blog:
https://sameerkapps.wordpress.com/2016/02/01/secure-storage-plugin-for-xamarin/

# Breaking Changes in 2.5.0
* iOS - The KeyChain access level is now set at AfterFirstUnlock as default. This will let app set the values
in the background mode.
As a result, the keys stored using the earlier versions will not be accessible with this version. To maintain backward compatibility,
add a line to AppDelegate as follows:
```
SecureStorageImplementation.DefaultAccessible = Security.SecAccessible.Invalid;
````

* Android - This version provides two storage types:
1. Android Key Store - This is now the default mechanism for storage. Android recommends using it as it prevents other apps from using the file.
ref: https://developer.android.com/training/articles/keystore#WhichShouldIUse
This is not compatible with the currently stored values.

2. Password Protected File - This is provided for the backward compatibility. If you want to use this mode, set it as follows:
```
SecureStorageImplementation.StorageType = StorageTypes.PasswordProtectedFile;
```
By default the password for storage is the hardware serial number. So it is unique per device. Should you want to choose your password, it can be done as follows
```
ProtectedFileImplementation.StoragePassword = "YourPassword";
```
Make sure that you obfuscate the app so the password is not reverse engineered.

# Changes in 2.0.0
* Abstraction Layer - Now compatible with .net standard
* iOS - No special changes
* Android - Hardware serial number is the default password
* UWP - New platform in 2.0.0. It has the following limitations.
    
        UWP Password vault has limitation of 10 values per app
        The value cannot be set as null or empty string. (Delete the key instead.)

* Mac - New platform in 2.0.0
* Tizen - New platform in 2.0.0
* Windows Phone 8.X - Retired. If you want to use it, it is there in 1.2.2 

# License
MIT License with the following override:
The source code can not be used to create another NuGet package for public distribution. (Private distribution within your organization is OK.)








