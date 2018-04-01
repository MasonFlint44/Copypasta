using System.Windows;

namespace Copypasta.DataAccess.Extensions
{
    public static class IDataObjectExtension
    {
        public static DataObject CopyToDataObject(this IDataObject source)
        {
            var clone = new DataObject();
            var formats = source.GetFormats() ?? new string[0];
            foreach (var format in formats)
            {
                var data = source.GetData(format);
                if (data != null)
                {
                    clone.SetData(format, data);
                }
            }
            return clone;
        }
    }
}
