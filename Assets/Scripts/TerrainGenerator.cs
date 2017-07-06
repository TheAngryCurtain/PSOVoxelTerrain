using UnityEngine;
using System.Collections;
using SimplexNoise;

public class TerrainGenerator
{
    private static int seed = 0;

    private static BlockAir BLOCK_AIR;
    private static BlockStone BLOCK_STONE;
    private static BlockGrass BLOCK_GRASS;
    private static BlockDirt BLOCK_DIRT;
    private static BlockLeaf BLOCK_LEAF;
    private static BlockWood BLOCK_WOOD;

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
        BLOCK_AIR = new BlockAir();
        BLOCK_STONE = new BlockStone();
        BLOCK_DIRT = new BlockDirt();
        BLOCK_GRASS = new BlockGrass();
        BLOCK_LEAF = new BlockLeaf();
        BLOCK_WOOD = new BlockWood();
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
        stoneHeight += GetNoise(x, 0, z, stoneMountainFrequency, SimplexNoise.Noise.FastFloor(stoneMountainHeight));

        if (stoneHeight < stoneMinHeight)
        {
            stoneHeight = SimplexNoise.Noise.FastFloor(stoneMinHeight);
        }

        stoneHeight += GetNoise(x, 0, z, stoneBaseNoise, SimplexNoise.Noise.FastFloor(stoneBaseNoiseHeight));
        int dirtHeight = stoneHeight + SimplexNoise.Noise.FastFloor(dirtBaseHeight);
        dirtHeight += GetNoise(x, 100, z, dirtNoise, SimplexNoise.Noise.FastFloor(dirtNoiseHeight));

        // starting at half the chunk height, and count up chunk height units, accounting for world pos
        int start = (int)(chunk.worldPos.y - (Chunk.chunkSize / 2f));
        int end = (int)chunk.worldPos.y + Chunk.chunkSize;
        for (int y = start; y < end; y++)
        {
            //Get a value to base cave generation on
            //int caveChance = GetNoise(x, y, z, caveFrequency, 100);
            if (y <= stoneHeight )//&& caveSize < caveChance)
            {
                SetBlock(x, y, z, BLOCK_STONE, chunk);
            }
            else if (y == dirtHeight )//&& caveSize < caveChance)
            {
                SetBlock(x, y, z, BLOCK_GRASS, chunk);

                // trees
                int treeChance = GetNoise(x, 0, z, treeFrequency, 100);
                if (y == dirtHeight && treeChance < treeDensity)
                {
                    CreateTree(x, y + 1, z, chunk);
                }
            }
            else if (y < dirtHeight) //&& caveSize < caveChance)
            {
                SetBlock(x, y, z, BLOCK_DIRT, chunk);
            }
            else
            {
                SetBlock(x, y, z, BLOCK_AIR, chunk);
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
                    SetBlock(x + xi, y + yi, z + zi, BLOCK_LEAF, chunk, true);
                }
            }
        }

        //create trunk
        for (int yt = 0; yt < 6; yt++)
        {
            SetBlock(x, y + yt, z, BLOCK_WOOD, chunk, true);
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
        return (int)((Noise.Generate(x * scale, (y + seed) * scale, z * scale) + 1f) * (max / 2f));
    }
}
