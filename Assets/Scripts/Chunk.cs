﻿using UnityEngine;
using System.Collections;

public class Chunk : MonoBehaviour
{
    public static int chunkSize = 16;

    private MeshData meshData;

    public bool update = false;
    public bool rendered = false;
    public bool built = false;
    public World world;
    public Vector3 worldPos;
    public bool inPool;

    [SerializeField] private MeshFilter filter;
    [SerializeField] private MeshCollider coll;

    public Block[,,] blocks = new Block[chunkSize, chunkSize, chunkSize];

    public void UpdateMesh()
    {
        meshData = new MeshData();
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    meshData = blocks[x, y, z].Blockdata(this, x, y, z, meshData);
                }
            }
        }
    }

    public void SetBlocksUnmodified()
    {
        Block b = null;
        for (int i = 0; i < chunkSize; i++)
        {
            for (int j = 0; j < chunkSize; j++)
            {
                for (int k = 0; k < chunkSize; k++)
                {
                    b = blocks[i, j, k];
                    b.changed = false;
                }
            }
        }
    }

    public Block GetBlock(int x, int y, int z)
    {
        if (InRange(x, chunkSize) && InRange(y, chunkSize) && InRange(z, chunkSize))
        {
            return blocks[x, y, z];
        }

        // if it's not an index inside this chunk bounds, check to see that it's not in another chunk
        return world.GetBlock((int)worldPos.x + x, (int)worldPos.y + y, (int)worldPos.z + z);
    }

    public void SetBlock(int x, int y, int z, Block block)
    {
        if (InRange(x, chunkSize) && InRange(y, chunkSize) && InRange(z, chunkSize))
        {
            blocks[x, y, z] = block;
        }
        else
        {
            world.SetBlock((int)worldPos.x + x, (int)worldPos.y + y, (int)worldPos.z + z, block);
        }
    }

    public static bool InRange(int index, int max)
    {
        if (index < 0 || index >= max)
        {
            return false;
        }

        return true;
    }

    public void RenderMesh()
    {
        RenderMesh(meshData);
        rendered = true;
    }

    // Sends the calculated mesh information
    // to the mesh and collision components
    void RenderMesh(MeshData mData)
    {
        filter.mesh.Clear();
        filter.mesh.vertices = mData.vertices.ToArray();
        filter.mesh.triangles = mData.triangles.ToArray();

        filter.mesh.uv = mData.uv.ToArray();
        filter.mesh.RecalculateNormals();

        // collision
        coll.sharedMesh = null;
        Mesh mesh = new Mesh();
        mesh.vertices = mData.colVertices.ToArray();
        mesh.triangles = mData.colTriangles.ToArray();
        mesh.RecalculateNormals();

        coll.sharedMesh = mesh;
    }

    public void ClearChunk()
    {
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    blocks[x, y, z] = null;
                }
            }
        }

        filter.mesh.Clear();
        coll.sharedMaterial = null;
        rendered = false;
        built = false;
    }
}
