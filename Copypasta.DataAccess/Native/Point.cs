namespace Copypasta.DataAccess.Native
{
    // https://msdn.microsoft.com/en-us/library/windows/desktop/dd162807(v=vs.85).aspx
    internal struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
