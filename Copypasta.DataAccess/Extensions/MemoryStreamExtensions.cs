using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Copypasta.DataAccess.Extensions
{
    public static class MemoryStreamExtensions
    {
        public static T ReadStruct<T>(this MemoryStream stream)
        {
            if(!typeof(T).IsValueType) { throw new NotSupportedException("Type parameter T must be a struct");}

            var bytesPtr = IntPtr.Zero;
            try
            {
                // Copy to byte array
                var bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);

                // Copy to unmanaged memory
                bytesPtr = Marshal.AllocHGlobal(bytes.Length);
                Marshal.Copy(bytes, 0, bytesPtr, bytes.Length);

                // Marshal unmanaged memory to struct
                return Marshal.PtrToStructure<T>(bytesPtr);
            }
            finally
            {
                if (bytesPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(bytesPtr);
                }
            }
        }
    }
}
