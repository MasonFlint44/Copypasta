using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using STATSTG = System.Runtime.InteropServices.ComTypes.STATSTG;

namespace Copypasta.DataAccess.Extensions
{
    public static class StgMediumExtensions
    {
        // https://stackoverflow.com/questions/24985239/dropped-zip-file-causes-e-data-getdatafilecontents-to-throw-an-exception
        public static MemoryStream GetMemoryStream(this STGMEDIUM medium)
        {
            if (medium.tymed != TYMED.TYMED_ISTREAM) { throw new NotSupportedException(); }

            // Marshal pointer to IStream object
            var stream = (IStream) Marshal.GetObjectForIUnknown(medium.unionmember);
            //Marshal.Release(medium.unionmember);

            // Get size of IStream
            var stat = new STATSTG();
            stream.Stat(out stat, 0);
            var size = (int) stat.cbSize;

            // Read IStream into byte array
            var content = new byte[size];
            stream.Read(content, size, IntPtr.Zero);

            // Wrap byte array in MemoryStream
            return new MemoryStream(content);
        }
    }
}
