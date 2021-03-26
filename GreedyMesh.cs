using System.Collections.Generic;

public static class GreedyMesh
{
    public static List<MeshData> Reduce(ushort[] blocks, int width, int height)
    {
        bool[] visited = new bool[blocks.Length];
        List<MeshData> compressedChunk = new List<MeshData>();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width - 1; x++)
            {
                ushort currentBlock = blocks[x * width + y];
                int xCounter = 1;
                if(visited[x * width + y]) continue;

                ushort nextBlock = blocks[(x + xCounter) * width + y];


                while (currentBlock == nextBlock) {
                    visited[(x + xCounter) * width + y] = true;
                    xCounter++;
                    if(x + xCounter >= width) break;
                    nextBlock = blocks[(x + xCounter) * width + y];
                }

                visited[x * width + y] = true;
                int yCounter = CheckY(x, y, xCounter, width, height, blocks, visited);
                compressedChunk.Add(new MeshData(currentBlock, xCounter, yCounter));
            }
        }

        compressedChunk.Add(new MeshData(2, 1, 1));

        return compressedChunk;
    }

    private static int CheckY(int startX, int startY, int xCounter, int width, int height, ushort[] blocks, bool[] visited)
    {
        int yCounter = 1;
        int[] yCounters = new int[xCounter];
        for (int i = 0; i < yCounters.Length; i++) yCounters[i] = 1;


        for (int x = startX; x < startX + xCounter; x++)
        {
            ushort currentBlock = blocks[x * width + startY];
            ushort nextBlock = blocks[x * width + (startY + yCounter)];
            bool canEscape = true;

            while(currentBlock == nextBlock)
            {
                visited[x * width + (startY + yCounter)] = true;
                yCounter++;
                if(startY + yCounter >= height) break;
                nextBlock = blocks[x * width + (startY + yCounter)];
                canEscape = false;
            }

            if(canEscape) return 1;
            yCounters[x - startX] = yCounter;
        }

        return MinOf(yCounters);
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