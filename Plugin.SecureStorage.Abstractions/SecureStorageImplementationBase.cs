// Copyrights Sameeer Khandekar
// This code is for viewing only. 
// If you need it to use for any other reason, please contact the author.

using System;

namespace Plugin.SecureStorage.Abstractions
{
	/// <summary>
	/// This base class provides validation functionality that is common across platforms
	/// </summary>
	internal abstract class SecureStorageImplementationBase : ISecureStorage
	{
        /// <summary>
        /// Default constructor
        /// </summary>
		public SecureStorageImplementationBase ()
		{
		}

		#region ISecureStorage implementation

		/// <summary>
		/// Validates the key
		/// </summary>
		/// <returns>null</returns>
		/// <param name="key">Key.</param>
		/// <param name="defaultValue">Default value.</param>
		public virtual string GetValue (string key, string defaultValue = null)
		{
			if (string.IsNullOrWhiteSpace (key))
			{
				throw new ArgumentException ("Invalid parameter: " + nameof (key));
			}

			return null;
		}

		/// <summary>
		/// Validates that key is not whitespace or null.
		/// And value is not null
		/// </summary>
		/// <returns>false</returns>
		public virtual bool SetValue (string key, string value)
		{
			if (string.IsNullOrWhiteSpace (key))
			{
				throw new ArgumentException ("Invalid parameter: " + nameof (key));
			}

			if (value == null)
			{
				throw new ArgumentNullException (nameof (value) + " cannot be null.");
			}

			return false;
		}

		/// <summary>
		/// Validates that key is not whitespace or null.
		/// </summary>
		/// <returns>false</returns>
		public virtual bool DeleteKey (string key)
		{
			if (string.IsNullOrWhiteSpace (key))
			{
				throw new ArgumentException ("Invalid parameter: " + nameof (key));
			}

			return false;
		}

		/// <summary>
		/// Validates that key is not whitespace or null.
		/// </summary>
		/// <returns>false</returns>
		public virtual bool HasKey (string key)
		{
			if (string.IsNullOrWhiteSpace (key))
			{
				throw new ArgumentException ("Invalid parameter: " + nameof (key));
			}

			return false;
		}

		#endregion
	}
}

