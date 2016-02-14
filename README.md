# Secure Storage Plugin for Xamarin and Windows Apps (Phone, Store, UWP)
The plugin can be used to **Securely store** sensitive data strings such as password, session token, credit card number etc. This plugin securely stores the strings across sessions on iOS, Android and Windows Apps including WP8, Store and UWP. 

It has easy to use API of key-value pairs. The pairs are saved using platform specific encryption mechanism. It has no dependencies on any packages (including Xamarin.Forms, MVVMCross.). And can be used by any Xamarin or Windows app. It is open source.

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

NOTE: In Android Apps, it is required that the password is set by the application prior to use.

```
SecureStorageImplementation.StoragePassword = "Your Password";
```

In Windows Apps, it is required that the password is set by the application prior to use.

```
WinSecureStorageBase.StoragePassword = "Your password";
```

**Breaking change in 1.2 for Windows Phone 8.0 app.** The following line 

~~
**SecureStorageImplementation**.StoragePassword = "Your Password";
~~

should be replaced with the line for all Windows Apps (as above)

The sample apps shows how to use it in Xamarin and in Windows Apps.

Plugin: http://www.nuget.org/packages/sameerIOTApps.Plugin.SecureStorage/

Blog: https://sameerkapps.wordpress.com/2016/02/01/secure-storage-plugin-for-xamarin/

#License
MIT License. 









