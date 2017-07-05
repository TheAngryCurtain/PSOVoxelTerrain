using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildJob : ThreadedJob
{
    private List<World.ChunkData> queue;
    private TerrainGenerator tGenerator;

    public void Setup()
    {
        if (tGenerator == null)
        {
            queue = new List<World.ChunkData>();
            tGenerator = new TerrainGenerator();
        }
    }

    public void SetQueue(List<World.ChunkData> other)
    {
        for (int i = 0; i < other.Count; i++)
        {
            queue.Add(other[i]);
        }
    }

    // threaded task. Don't use Unity API here
    protected override void ThreadFunction()
    {
        Chunk current;
        //List<World.ChunkData> temp = new List<World.ChunkData>(queue.Count);
        for (int i = 0; i < queue.Count; i++)
        {
            current = queue[i].Chunk;
            current.worldPos = queue[i].WorldPosition;
            current.world = queue[i].World;

            current = tGenerator.ChunkGen(current);
            current.SetBlocksUnmodified();

            // tell it that it needs to update
            current.DrawChunk();

            // store a copy to return
            //temp.Add(queue[i]);

            //Serialization.Load(current);
        }

        // save the data here so that it can be returned in the JobComplete action
        // need to store it in temp because otherwise the data gets cleared when the queue does
        //_data = (object)temp;

        queue.Clear();
    }
}
