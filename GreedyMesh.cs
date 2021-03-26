using System;
using System.Collections.Generic;

public static class GreedyMesh
{
    public static List<MeshData> Reduce(ushort[] blocks, int width, int height)
    {
        bool[] visited = new bool[blocks.Length];
        List<MeshData> compressedChunk = new List<MeshData>();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                ushort currentBlock = blocks[x * width + y];
                int xCounter = 1;
                if(visited[x * width + y]) continue;

                if (x != width - 1)
                {
                    ushort nextBlock = blocks[(x + xCounter) * width + y];


                    while (currentBlock == nextBlock) {
                        xCounter++;
                        if(x + xCounter >= width) break;
                        if (visited[(x + xCounter) * width + y]) break;
                        nextBlock = blocks[(x + xCounter) * width + y];
                    }
                }

                int yCounter = CheckY(x, y, xCounter, width, height, blocks);

                MeshData data = new MeshData(currentBlock, xCounter, yCounter);

                MarkVisited(x, y, data.Width, data.Height, visited, width);
                
                compressedChunk.Add(data);
                DEBUGDRAW(data, width, compressedChunk.Count - 1);
            }
        }
        
        return compressedChunk;
    }
    
    private static int CheckY(int startX, int startY, int xCounter, int width, int height, ushort[] blocks)
    {
        int[] yCounters = new int[xCounter];
        for (int i = 0; i < yCounters.Length; i++) yCounters[i] = 1;


        for (int x = startX; x < startX + xCounter; x++)
        {
            int yCounter = 1;
            ushort currentBlock = blocks[x * width + startY];

            if (startY + yCounter < width)
            {
                ushort nextBlock = blocks[x * width + (startY + yCounter)];
                bool canEscape = true;

                while(currentBlock == nextBlock)
                {
                    yCounter++;
                    if(startY + yCounter >= height) break;
                    nextBlock = blocks[x * width + (startY + yCounter)];
                    canEscape = false;
                }

                if(canEscape) return 1;
            }
            yCounters[x - startX] = yCounter;
        }

        return MinOf(yCounters);
    }

    private static void MarkVisited(int startX, int startY, int width, int height, bool[] visited, int absoluteWidth)
    {
        for (int y = startY; y < startY + height; y++)
        {
            for (int x = startX; x < startX + width; x++)
            {
                visited[x * absoluteWidth + y] = true;
            }
        }
    }
    
    private static void DEBUGDRAW(MeshData data, int width, int y)
    {
        Console.SetCursorPosition((width - 1) * 2 + 4, y);
        Console.Write($"Block: ");
        Console.ForegroundColor = PickColor(data.Block);
        Console.Write(data.Block);
        Console.ResetColor();
        Console.Write($" Width: {data.Width} \t Height: {data.Height}");
    }
    
    private static void DEBUGDRAW(string value, int width, int y)
    {
        Console.SetCursorPosition((width - 1) * 2 + 4, y);
        Console.ResetColor();
        Console.Write(value);
    }
    
    private static ConsoleColor PickColor(ushort block) 
    {
        return (ConsoleColor) (block + 2);
    }

    private static int MinOf(int[] values)
    {
        int minValue = int.MaxValue;

        for (int i = 0; i < values.Length; i++)
        {
            if(values[i] < minValue)
                minValue = (int) values[i];
        }

        return minValue;
    }
}