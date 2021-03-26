using System;
using System.Collections.Generic;

namespace NaiveGreedyMeshAlgorithm
{
    class Program
    {
        public static void Main(string[] args)
        {
            if(Environment.OSVersion.Platform == PlatformID.Win32NT)
                Console.SetBufferSize(Console.BufferWidth, 200);
            
            const int width = 16;
            const int height = 16;
            ushort[] chunk = new ushort[width * height];
            PopulateChunk(chunk, width, height);

            DrawOnConsole(chunk, width, height);
            DrawVerticalLine((width - 1) * 2 + 2);
            List<MeshData> data = GreedyMesh.Reduce(chunk, width, height);
            DrawOnConsole(data, width, height);
            Console.Write("Reduced size from: " + width * height + " to " + data.Count);
            Console.ReadKey();
        }
        private static void PopulateChunk(ushort[] chunk, int width, int height)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float noise = SimplexNoise.Noise.CalcPixel2D(x, y, 0.1f);
                    float normalizedNoise = noise / 255.0f;
                    ushort block = (ushort) (normalizedNoise * 5.0f);
                    chunk[x * width + y] = block;
                }
            }
        }

        private static void DrawOnConsole(ushort[] blocks, int width, int height)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Console.SetCursorPosition(x * 2, y);
                    ushort block = blocks[x * width + y];
                    Console.ForegroundColor = PickColor(block);
                    Console.Write(block);
                }
            }
        }

        private static void DrawVerticalLine(int x)
        {
            Console.ResetColor();
            for (int i = 0; i < 16; i++)
            {
                Console.SetCursorPosition(x, i);
                Console.Write("|");
            }
        }


        private static ConsoleColor PickColor(ushort block) 
        {
            return (ConsoleColor) (block + 2);
        }

        private static void DrawOnConsole(List<MeshData> meshData, int width, int height)
        {
            for (int y = 0; y < meshData.Count; y++)
            {
                MeshData data = meshData[y];
                Console.SetCursorPosition((width - 1) * 2 + 4, y);
                Console.Write($"Block: ");
                Console.ForegroundColor = PickColor(data.Block);
                Console.Write(data.Block);
                Console.ResetColor();
                Console.Write($" Width: {data.Width} \t Height: {data.Height}");
            }

            Console.SetCursorPosition(0, height);
        }
    }
}
