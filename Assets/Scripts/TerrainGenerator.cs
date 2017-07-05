using UnityEngine;
using System.Collections;
using SimplexNoise;

public class TerrainGenerator
{
    int seed = 0;

    private float stoneBaseHeight = -24;
    private float stoneBaseNoise = 0.05f;
    private float stoneBaseNoiseHeight = 4;
    private float stoneMountainHeight = 48;
    private float stoneMountainFrequency = 0.008f;
    private float stoneMinHeight = -12;
    private float dirtBaseHeight = 1;
    private float dirtNoise = 0.04f;
    private float dirtNoiseHeight = 3;

    private float caveFrequency = 0.025f;
    private int caveSize = 7;

    private float treeFrequency = 0.2f;
    private int treeDensity = 3;

    public void Init()
    {
        //seed = UnityEngine.Random.Range(0, int.MaxValue);
    }

    public Chunk ChunkGen(Chunk chunk)
    {
        for (int x = (int)chunk.worldPos.x - 3; x < chunk.worldPos.x + Chunk.chunkSize + 3; x++)
        {
            for (int z = (int)chunk.worldPos.z - 3; z < chunk.worldPos.z + Chunk.chunkSize + 3; z++)
            {
                chunk = ChunkColumnGen(chunk, x, z);
            }
        }

        return chunk;
    }

    public Chunk ChunkColumnGen(Chunk chunk, int x, int z)
    {
        int stoneHeight = SimplexNoise.Noise.FastFloor(stoneBaseHeight);
        stoneHeight += GetNoise(x, 0 + seed, z, stoneMountainFrequency, SimplexNoise.Noise.FastFloor(stoneMountainHeight));

        if (stoneHeight < stoneMinHeight)
        {
            stoneHeight = SimplexNoise.Noise.FastFloor(stoneMinHeight);
        }

        stoneHeight += GetNoise(x, 0 + seed, z, stoneBaseNoise, SimplexNoise.Noise.FastFloor(stoneBaseNoiseHeight));
        int dirtHeight = stoneHeight + SimplexNoise.Noise.FastFloor(dirtBaseHeight);
        dirtHeight += GetNoise(x, 100 + seed, z, dirtNoise, SimplexNoise.Noise.FastFloor(dirtNoiseHeight));

        // starting at half the chunk height, and count up chunk height units, accounting for world pos
        for (int y = (int)(chunk.worldPos.y - (Chunk.chunkSize / 2f)); y < chunk.worldPos.y + Chunk.chunkSize; y++)
        {
            //Get a value to base cave generation on
            int caveChance = GetNoise(x, y + seed, z, caveFrequency, 100);
            if (y <= stoneHeight )//&& caveSize < caveChance)
            {
                SetBlock(x, y, z, new BlockStone(), chunk);
            }
            else if (y == dirtHeight )//&& caveSize < caveChance)
            {
                SetBlock(x, y, z, new BlockGrass(), chunk);

                // trees
                int treeChance = GetNoise(x, 0 + seed, z, treeFrequency, 100);
                if (y == dirtHeight && treeChance < treeDensity)
                {
                    CreateTree(x, y + 1, z, chunk);
                }
            }
            else if (y < dirtHeight) //&& caveSize < caveChance)
            {
                SetBlock(x, y, z, new BlockDirt(), chunk);
            }
            else
            {
                SetBlock(x, y, z, new BlockAir(), chunk);
            }
        }

        return chunk;
    }

    private void CreateTree(int x, int y, int z, Chunk chunk)
    {
        //create leaves
        for (int xi = -2; xi <= 2; xi++)
        {
            for (int yi = 4; yi <= 8; yi++)
            {
                for (int zi = -2; zi <= 2; zi++)
                {
                    SetBlock(x + xi, y + yi, z + zi, new BlockLeaf(), chunk, true);
                }
            }
        }

        //create trunk
        for (int yt = 0; yt < 6; yt++)
        {
            SetBlock(x, y + yt, z, new BlockWood(), chunk, true);
        }
    }

    public static void SetBlock(int x, int y, int z, Block block, Chunk chunk, bool replaceBlocks = false)
    {
        x -= (int)chunk.worldPos.x;
        y -= (int)chunk.worldPos.y;
        z -= (int)chunk.worldPos.z;
        if (Chunk.InRange(x, Chunk.chunkSize) && Chunk.InRange(y, Chunk.chunkSize) && Chunk.InRange(z, Chunk.chunkSize))
        {
            if (replaceBlocks || chunk.blocks[x, y, z] == null)
            {
                chunk.SetBlock(x, y, z, block);
            }
        }
    }

    public static int GetNoise(int x, int y, int z, float scale, int max)
    {
        return Mathf.FloorToInt((Noise.Generate(x * scale, y * scale, z * scale) + 1f) * (max / 2f));
    }
}
