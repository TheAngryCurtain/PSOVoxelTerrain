using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChunkLoader : MonoBehaviour
{
    public World world;
    public Transform cachedTransform;

    private List<Vector3> updateList = new List<Vector3>();
    private List<Vector3> buildList = new List<Vector3>();
    private int timer = 0;

    private List<Vector3> chunksToDelete = new List<Vector3>();
    private Vector3 deleteCheckVector3 = Vector3.zero;
    private Vector3 currentPlayerVector3 = Vector3.zero;
    private float sqrCompareDist = 50 * 50;

    private static Vector3[] chunkPositions =
        {
        new Vector3( 0, 0,  0), new Vector3(-1, 0,  0), new Vector3( 0, 0, -1), new Vector3( 0, 0,  1), new Vector3( 1, 0,  0),
        new Vector3(-1, 0, -1), new Vector3(-1, 0,  1), new Vector3( 1, 0, -1), new Vector3( 1, 0,  1), new Vector3(-2, 0,  0),
        new Vector3( 0, 0, -2), new Vector3( 0, 0,  2), new Vector3( 2, 0,  0), new Vector3(-2, 0, -1), new Vector3(-2, 0,  1),
        new Vector3(-1, 0, -2), new Vector3(-1, 0,  2), new Vector3( 1, 0, -2), new Vector3( 1, 0,  2), new Vector3( 2, 0, -1),
        new Vector3( 2, 0,  1)/*, new Vector3(-2, 0, -2), new Vector3(-2, 0,  2), new Vector3( 2, 0, -2), new Vector3( 2, 0,  2),
        new Vector3(-3, 0,  0), new Vector3( 0, 0, -3), new Vector3( 0, 0,  3), new Vector3( 3, 0,  0), new Vector3(-3, 0, -1),
        new Vector3(-3, 0,  1), new Vector3(-1, 0, -3), new Vector3(-1, 0,  3), new Vector3( 1, 0, -3), new Vector3( 1, 0,  3),
        new Vector3( 3, 0, -1), new Vector3( 3, 0,  1), new Vector3(-3, 0, -2), new Vector3(-3, 0,  2), new Vector3(-2, 0, -3),
        new Vector3(-2, 0,  3), new Vector3( 2, 0, -3), new Vector3( 2, 0,  3), new Vector3( 3, 0, -2), new Vector3( 3, 0,  2),
        new Vector3(-4, 0,  0), new Vector3( 0, 0, -4), new Vector3( 0, 0,  4), new Vector3( 4, 0,  0), new Vector3(-4, 0, -1),
        new Vector3(-4, 0,  1), new Vector3(-1, 0, -4), new Vector3(-1, 0,  4), new Vector3( 1, 0, -4), new Vector3( 1, 0,  4),
        new Vector3( 4, 0, -1), new Vector3( 4, 0,  1), new Vector3(-3, 0, -3), new Vector3(-3, 0,  3), new Vector3( 3, 0, -3),
        new Vector3( 3, 0,  3), new Vector3(-4, 0, -2), new Vector3(-4, 0,  2), new Vector3(-2, 0, -4), new Vector3(-2, 0,  4),
        new Vector3( 2, 0, -4), new Vector3( 2, 0,  4), new Vector3( 4, 0, -2), new Vector3( 4, 0,  2), new Vector3(-5, 0,  0),
        new Vector3(-4, 0, -3), new Vector3(-4, 0,  3), new Vector3(-3, 0, -4), new Vector3(-3, 0,  4), new Vector3( 0, 0, -5),
        new Vector3( 0, 0,  5), new Vector3( 3, 0, -4), new Vector3( 3, 0,  4), new Vector3( 4, 0, -3), new Vector3( 4, 0,  3),
        new Vector3( 5, 0,  0), new Vector3(-5, 0, -1), new Vector3(-5, 0,  1), new Vector3(-1, 0, -5), new Vector3(-1, 0,  5),
        new Vector3( 1, 0, -5), new Vector3( 1, 0,  5), new Vector3( 5, 0, -1), new Vector3( 5, 0,  1), new Vector3(-5, 0, -2),
        new Vector3(-5, 0,  2), new Vector3(-2, 0, -5), new Vector3(-2, 0,  5), new Vector3( 2, 0, -5), new Vector3( 2, 0,  5),
        new Vector3( 5, 0, -2), new Vector3( 5, 0,  2), new Vector3(-4, 0, -4), new Vector3(-4, 0,  4), new Vector3( 4, 0, -4),
        new Vector3( 4, 0,  4), new Vector3(-5, 0, -3), new Vector3(-5, 0,  3), new Vector3(-3, 0, -5), new Vector3(-3, 0,  5),
        new Vector3( 3, 0, -5), new Vector3( 3, 0,  5), new Vector3( 5, 0, -3), new Vector3( 5, 0,  3), new Vector3(-6, 0,  0),
        new Vector3( 0, 0, -6), new Vector3( 0, 0,  6), new Vector3( 6, 0,  0), new Vector3(-6, 0, -1), new Vector3(-6, 0,  1),
        new Vector3(-1, 0, -6), new Vector3(-1, 0,  6), new Vector3( 1, 0, -6), new Vector3( 1, 0,  6), new Vector3( 6, 0, -1),
        new Vector3( 6, 0,  1), new Vector3(-6, 0, -2), new Vector3(-6, 0,  2), new Vector3(-2, 0, -6), new Vector3(-2, 0,  6),
        new Vector3( 2, 0, -6), new Vector3( 2, 0,  6), new Vector3( 6, 0, -2), new Vector3( 6, 0,  2), new Vector3(-5, 0, -4),
        new Vector3(-5, 0,  4), new Vector3(-4, 0, -5), new Vector3(-4, 0,  5), new Vector3( 4, 0, -5), new Vector3( 4, 0,  5),
        new Vector3( 5, 0, -4), new Vector3( 5, 0,  4), new Vector3(-6, 0, -3), new Vector3(-6, 0,  3), new Vector3(-3, 0, -6),
        new Vector3(-3, 0,  6), new Vector3( 3, 0, -6), new Vector3( 3, 0,  6), new Vector3( 6, 0, -3), new Vector3( 6, 0,  3),
        new Vector3(-7, 0,  0), new Vector3( 0, 0, -7), new Vector3( 0, 0,  7), new Vector3( 7, 0,  0), new Vector3(-7, 0, -1),
        new Vector3(-7, 0,  1), new Vector3(-5, 0, -5), new Vector3(-5, 0,  5), new Vector3(-1, 0, -7), new Vector3(-1, 0,  7),
        new Vector3( 1, 0, -7), new Vector3( 1, 0,  7), new Vector3( 5, 0, -5), new Vector3( 5, 0,  5), new Vector3( 7, 0, -1),
        new Vector3( 7, 0,  1), new Vector3(-6, 0, -4), new Vector3(-6, 0,  4), new Vector3(-4, 0, -6), new Vector3(-4, 0,  6),
        new Vector3( 4, 0, -6), new Vector3( 4, 0,  6), new Vector3( 6, 0, -4), new Vector3( 6, 0,  4), new Vector3(-7, 0, -2),
        new Vector3(-7, 0,  2), new Vector3(-2, 0, -7), new Vector3(-2, 0,  7), new Vector3( 2, 0, -7), new Vector3( 2, 0,  7),
        new Vector3( 7, 0, -2), new Vector3( 7, 0,  2), new Vector3(-7, 0, -3), new Vector3(-7, 0,  3), new Vector3(-3, 0, -7),
        new Vector3(-3, 0,  7), new Vector3( 3, 0, -7), new Vector3( 3, 0,  7), new Vector3( 7, 0, -3), new Vector3( 7, 0,  3),
        new Vector3(-6, 0, -5), new Vector3(-6, 0,  5), new Vector3(-5, 0, -6), new Vector3(-5, 0,  6), new Vector3( 5, 0, -6),
        new Vector3( 5, 0,  6), new Vector3( 6, 0, -5), new Vector3( 6, 0,  5)*/
    };

    // TODO
    // this is currently on the player object, and should be moved and use events to spawn this stuff

    private void Awake()
    {
        cachedTransform = this.transform;
        world.Init(300);
    }

    private void Update()
    {
        bool didDelete = DeleteChunks();
        if (didDelete)
        {
            return;
        }

        FindChunksToLoad();
        LoadAndRenderChunks();
    }

    private void FindChunksToLoad()
    {
        //Get the position of this gameobject to generate around
        Vector3 playerPos = new Vector3(
            SimplexNoise.Noise.FastFloor(cachedTransform.position.x / Chunk.chunkSize) * Chunk.chunkSize,
            SimplexNoise.Noise.FastFloor(cachedTransform.position.y / Chunk.chunkSize) * Chunk.chunkSize,
            SimplexNoise.Noise.FastFloor(cachedTransform.position.z / Chunk.chunkSize) * Chunk.chunkSize
            );

        //If there aren't already chunks to generate
        if (updateList.Count == 0)
        {
            //Cycle through the array of positions
            for (int i = 0; i < chunkPositions.Length; i++)
            {
                //translate the player position and array position into chunk position
                Vector3 newChunkPos = new Vector3(chunkPositions[i].x * Chunk.chunkSize + playerPos.x, 0, chunkPositions[i].z * Chunk.chunkSize + playerPos.z);

                // if the chunk is too far away, don't try to build it
                if (IsChunkOutOfRange(newChunkPos))
                {
                    continue;
                }

                //Get the chunk in the defined position
                Chunk newChunk = world.GetChunk((int)newChunkPos.x, (int)newChunkPos.y, (int)newChunkPos.z);

                //If the chunk already exists and it's already
                //rendered or in queue to be rendered continue
                if (newChunk != null && (newChunk.rendered || updateList.Contains(newChunkPos)))
                {
                    continue;
                }

                //load a column of chunks in this position
                for (int y = -4; y < 4; y++)
                {
                    for (int x = (int)newChunkPos.x - Chunk.chunkSize; x <= newChunkPos.x + Chunk.chunkSize; x += Chunk.chunkSize)
                    {
                        for (int z = (int)newChunkPos.z - Chunk.chunkSize; z <= newChunkPos.z + Chunk.chunkSize; z += Chunk.chunkSize)
                        {
                            buildList.Add(new Vector3(x, y * Chunk.chunkSize, z));
                        }
                    }

                    updateList.Add(new Vector3(newChunkPos.x, y * Chunk.chunkSize, newChunkPos.z));
                }

                return;
            }
        }
    }

    void BuildChunk(Vector3 pos)
    {
        int posX = (int)pos.x;
        int posY = (int)pos.y;
        int posZ = (int)pos.z;
        if (world.GetChunk(posX, posY, posZ) == null)
        {
            world.CreateChunk(posX, posY, posZ);
        }
    }

    private bool IsChunkOutOfRange(Vector3 chunkVector3)
    {
        deleteCheckVector3.x = chunkVector3.x;
        deleteCheckVector3.z = chunkVector3.z;

        currentPlayerVector3.x = cachedTransform.position.x;
        currentPlayerVector3.z = cachedTransform.position.z;

        float sqrDist = (deleteCheckVector3 - currentPlayerVector3).sqrMagnitude;
        return (sqrDist > sqrCompareDist);
    }

    private bool DeleteChunks()
    {
        if (timer == 10)
        {
            chunksToDelete.Clear();
            foreach (var chunk in world.chunks)
            {
                if (IsChunkOutOfRange(chunk.Value.worldPos))
                {
                    chunksToDelete.Add(chunk.Key);
                }
            }

            Vector3 wp;
            for (int i = 0; i < chunksToDelete.Count; i++)
            {
                wp = chunksToDelete[i];
                world.DestroyChunk((int)wp.x, (int)wp.y, (int)wp.z);
            }

            timer = 0;
            return true;
        }

        timer++;
        return false;
    }

    void LoadAndRenderChunks()
    {
        if (buildList.Count > 0)
        {
            for (int i = 0; i < buildList.Count && i < 8; i++)
            {
                BuildChunk(buildList[0]);
                buildList.RemoveAt(0);
            }

            //If chunks were built return early
            return;
        }

        if (updateList.Count != 0)
        {
            Chunk chunk = world.GetChunk((int)updateList[0].x, (int)updateList[0].y, (int)updateList[0].z);
            if (chunk != null)
            {
                chunk.update = true;
            }

            updateList.RemoveAt(0);
        }
    }
}
