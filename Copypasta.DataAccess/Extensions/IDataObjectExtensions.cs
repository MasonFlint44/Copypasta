using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Windows;
using Copypasta.DataAccess.Native;
using IDataObject = System.Windows.IDataObject;
using IComDataObject = System.Runtime.InteropServices.ComTypes.IDataObject;

namespace Copypasta.DataAccess.Extensions
{
    public static class IDataObjectExtension
    {
        public static DataObject Clone(this IDataObject source)
        {
            var dataObjCopy = new DataObject();
            var formats = source.GetFormats() ?? new string[0];
            foreach (var format in formats)
            {
                try
                {
                    var data = source.GetData(format);
                    dataObjCopy.SetData(format, data);
                }
                catch
                {
                    // Do nothing
                }
            }
            return dataObjCopy;
        }

        // https://stackoverflow.com/questions/24985239/dropped-zip-file-causes-e-data-getdatafilecontents-to-throw-an-exception
        public static MemoryStream[] GetFileContents(this IDataObject source)
        {
            if (!source.GetDataPresent("FileGroupDescriptorW")) { throw new NotSupportedException("Cannot get FileContents without FileGroupDescriptorW or FileDescriptorW present."); }

            var fileGroupDescriptor = new FileGroupDescriptor((MemoryStream)source.GetData("FileGroupDescriptorW"));
            var streams = new MemoryStream[fileGroupDescriptor.Items];
            for (var i = 0; i < fileGroupDescriptor.Items; i++)
            {
                streams[i] = GetFileContentsMemoryStream((IComDataObject)source, i);
            }
            return streams;
        }

        private static MemoryStream GetFileContentsMemoryStream(IComDataObject dataObject, int index)
        {
            var dataFormat = DataFormats.GetDataFormat("FileContents");
            var formatEtc = new FORMATETC
            {
                cfFormat = (short)dataFormat.Id,
                dwAspect = DVASPECT.DVASPECT_CONTENT,
                lindex = -1,
                ptd = IntPtr.Zero,
                tymed = TYMED.TYMED_ISTREAM | TYMED.TYMED_HGLOBAL
            };
            var medium = new STGMEDIUM();
            dataObject.GetData(ref formatEtc, out medium);
            
            return medium.GetMemoryStream();
        }
    }
}
