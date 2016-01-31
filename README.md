# SecureStorage
The plugin can be used for storing sensitive data strings such as password, authentication token, credit card number etc. This plugin provides ability to securely store strings on iOS, Android and Windows Phone 8.0.
(Not supported on Windows Phone 8.1,  Windows 8.1 and UWP). 
The strings are stored as key-valy pairs. They are saved using platform specific encryption mechanism. So the data is stored securely and can be used across the sessions.

It provides 4 methods
1. SetValue - Stores the key and value.
2. GetValue - Returns the value for the given key. If not found, returns default value.
3. DeleteKey - Deletes the given key and corresponding value from the storage.
4. HasKey - Checks if the given key exists in the storage.

NOTE: In Android and Windows Phone 8.0, it is required that the password is set by the application prior to use.

The sample app shows how to use it in Xamarin.

#License
MIT License. 