﻿using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;

namespace Plugin.SecureStorage.Credentials
{
    internal sealed class CriticalCredentialHandle : CriticalHandleZeroOrMinusOneIsInvalid
    {
        // Set the handle.
        internal CriticalCredentialHandle(IntPtr preexistingHandle)
        {
            SetHandle (preexistingHandle);
        }

        internal Credential GetCredential()
        {
            if ( !IsInvalid )
            {
                // Get the Credential from the mem location
                NativeCode.NativeCredential ncred = (NativeCode.NativeCredential) Marshal.PtrToStructure (handle,
                      typeof (NativeCode.NativeCredential));

                // Create a managed Credential type and fill it with data from the native counterpart.
                Credential cred = new Credential (ncred);
   
                return cred;
            }
            else
            {
                throw new InvalidOperationException ("Invalid CriticalHandle!");
            }
        }

        // Perform any specific actions to release the handle in the ReleaseHandle method.
        // Often, you need to use Pinvoke to make a call into the Win32 API to release the 
        // handle. In this case, however, we can use the Marshal class to release the unmanaged memory.

        override protected bool ReleaseHandle()
        {
            // If the handle was set, free it. Return success.
            if ( !IsInvalid )
            {
                // NOTE: We should also ZERO out the memory allocated to the handle, before free'ing it
                // so there are no traces of the sensitive data left in memory.
                NativeCode.CredFree (handle);
                // Mark the handle as invalid for future users.
                SetHandleAsInvalid ();
                return true;
            }
            // Return false. 
            return false;
        }
    }
}
