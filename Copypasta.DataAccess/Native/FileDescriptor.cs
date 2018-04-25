using System;
using System.IO;
using System.Text;

namespace Copypasta.DataAccess.Native
{
    // https://msdn.microsoft.com/en-us/library/windows/desktop/bb773288(v=vs.85).aspx
    internal struct FileDescriptor
    {
        public FileDescriptorFlags Flags;
        public Guid ClassId;
        public Size Size;
        public Point Point;
        public FileAttributes FileAttributes;
        public DateTime CreationTime;
        public DateTime LastAccessTime;
        public DateTime LastWriteTime;
        public long FileSize;
        public string FileName;

        public FileDescriptor(Stream stream)
        {
            var reader = new BinaryReader(stream);

            Flags = (FileDescriptorFlags)reader.ReadUInt32();
            ClassId = new Guid(reader.ReadBytes(16));
            Size = new Size(reader.ReadInt32(), reader.ReadInt32());
            Point = new Point(reader.ReadInt32(), reader.ReadInt32());
            FileAttributes = (FileAttributes)reader.ReadUInt32();
            CreationTime = new DateTime(1601, 1, 1).AddTicks(reader.ReadInt64());
            LastAccessTime = new DateTime(1601, 1, 1).AddTicks(reader.ReadInt64());
            LastWriteTime = new DateTime(1601, 1, 1).AddTicks(reader.ReadInt64());
            FileSize = reader.ReadInt64();
            var nameBytes = reader.ReadBytes(520);
            var i = 0;
            while (i < nameBytes.Length)
            {
                if (nameBytes[i] == 0 && nameBytes[i + 1] == 0)
                    break;
                i += 2;
            }
            FileName = Encoding.Unicode.GetString(nameBytes, 0, i);
        }
    }
}
