public struct MeshData
{
    public ushort Block { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    public MeshData(ushort block, int width, int height)
    {
        this.Block = block;
        this.Width = width;
        this.Height = height;
    }
}