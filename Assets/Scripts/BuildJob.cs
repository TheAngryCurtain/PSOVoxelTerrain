using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildJob : ThreadedJob
{
    private List<World.ChunkData> queue;
    private TerrainGenerator tGenerator;
    private List<Chunk> finishedChunks = new List<Chunk>();

    public void Setup()
    {
        queue = new List<World.ChunkData>();
        tGenerator = new TerrainGenerator();

        tGenerator.Init();
    }

    public void SetQueue(List<World.ChunkData> other)
    {
        for (int i = 0; i < other.Count; i++)
        {
            queue.Add(other[i]);
        }
    }

    public List<Chunk> GetFinishedData()
    {
        return finishedChunks;
    }

    // threaded task. Don't use Unity API here
    protected override void ThreadFunction()
    {
        Chunk current;
        for (int i = 0; i < queue.Count; i++)
        {
            current = queue[i].Chunk;
            current.worldPos = queue[i].WorldPosition;
            current.world = queue[i].World;

            current = tGenerator.ChunkGen(current);
            current.SetBlocksUnmodified();

            // tell it that it needs to update
            //current.DrawChunk();

            // store a copy to return
            finishedChunks.Add(current);

            //Serialization.Load(current);
        }

        queue.Clear();
    }
}
