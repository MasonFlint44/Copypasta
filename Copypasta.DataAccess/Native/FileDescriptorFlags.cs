using System;

namespace Copypasta.DataAccess.Native
{
    // https://msdn.microsoft.com/en-us/library/windows/desktop/bb773288(v=vs.85).aspx
    [Flags]
    internal enum FileDescriptorFlags: uint
    {
        ClsId = 0x00000001,
        SizePoint = 0x00000002,
        Attributes = 0x00000004,
        CreateTime = 0x00000008,
        AccessTime = 0x00000010,
        WritesTime = 0x00000020,
        FileSize = 0x00000040,
        ProgressUi = 0x00004000,
        LinkUi = 0x00008000,
        Unicode = 0x80000000
    }
}
