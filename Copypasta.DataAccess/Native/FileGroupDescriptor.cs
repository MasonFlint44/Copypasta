using System.IO;

namespace Copypasta.DataAccess.Native
{
    // https://msdn.microsoft.com/en-us/library/windows/desktop/bb773290(v=vs.85).aspx
    internal struct FileGroupDescriptor
    {
        public uint Items;
        public FileDescriptor[] FileDescriptors;

        public FileGroupDescriptor(Stream stream)
        {
            var reader = new BinaryReader(stream);

            Items = reader.ReadUInt32();
            FileDescriptors = new FileDescriptor[Items];
            for(var i = 0; i < Items; i++)
            {
                FileDescriptors[i] = new FileDescriptor(stream);
            }
        }
    }
}
