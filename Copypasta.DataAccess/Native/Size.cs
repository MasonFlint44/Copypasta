namespace Copypasta.DataAccess.Native
{
    internal struct Size
    {
        public int Cx { get; set; }
        public int Cy { get; set; }

        public Size(int cx, int cy)
        {
            Cx = cx;
            Cy = cy;
        }
    }
}
