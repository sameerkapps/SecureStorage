# Secure Storage Xamarin Plugin

Supports Androi, iOS, UWP and Desktop Windows (.net 4.6.1 and up)

The plugin can be used to securely store sensitive data strings such as password using each platform's native keychain.

**Note**

If your app fails to save values in iOS 10 Simulator, open the Entitlements.plist file and make sure that "Enable Keychain Access Groups" is checked. Also ensure that in Project->Options->iOS Bundle Signing, the Entitlements.plist is selected in Custom Entitlements for iPhoneSimulator platform.
This happens only in iOS 10 Simulator due to https://forums.xamarin.com/discussion/77760/ios-10-keychain

# Usage

**SetValue** - Stores key-value pair

``` 
CrossSecureStorage.Current.SetValue(“SessionToken”, “1234567890”);
```

**GetValue** - Returns the value for the given key. If not found, returns default value.

``` 
var sessionToken = CrossSecureStorage.Current.GetValue (“SessionToken”);
```

**DeleteKey** - Deletes the key and corresponding value from the storage.

``` 
CrossSecureStorage.Current.DeleteKey(“SessionToken”);
``` 

**HasKey** - Checks if the key exists.

```
var exists = CrossSecureStorage.Current.HasKey (“SessionToken”);
``` 

NOTE: In Android Apps, it is required that the password is set by the application prior to use.

```
SecureStorageImplementation.StoragePassword = "Your Password";
```

# License

MIT License. 










