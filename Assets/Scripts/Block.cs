using UnityEngine;
using System.Collections;
using System;

public struct Tile
{
    public int x;
    public int y;
}

[Serializable]
public class Block
{
    public enum eFace { Top, Bottom, Front, Back, Left, Right };

    private const float TILE_SIZE = 0.25f;

    public bool changed = true;

    public Block()
    {

    }

    public virtual bool IsSolid(eFace side)
    {
        switch (side)
        {
            case eFace.Top:
                return true;
            case eFace.Bottom:
                return true;
            case eFace.Front:
                return true;
            case eFace.Back:
                return true;
            case eFace.Left:
                return true;
            case eFace.Right:
                return true;
        }
        return false;
    }

    public virtual Tile TexturePosition(eFace face)
    {
        Tile tile = new Tile();
        tile.x = 0;
        tile.y = 0;
        return tile;
    }

    public virtual Vector2[] FaceUVs(eFace face)
    {
        Vector2[] UVs = new Vector2[4];
        Tile tilePos = TexturePosition(face);
        UVs[0] = new Vector2(TILE_SIZE * tilePos.x + TILE_SIZE, TILE_SIZE * tilePos.y);
        UVs[1] = new Vector2(TILE_SIZE * tilePos.x + TILE_SIZE, TILE_SIZE * tilePos.y + TILE_SIZE);
        UVs[2] = new Vector2(TILE_SIZE * tilePos.x, TILE_SIZE * tilePos.y + TILE_SIZE);
        UVs[3] = new Vector2(TILE_SIZE * tilePos.x, TILE_SIZE * tilePos.y);
        return UVs;
    }

    public virtual MeshData Blockdata (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.useRenderDataForCollision = true;

        if (!chunk.GetBlock(x, y + 1, z).IsSolid(eFace.Bottom))
        {
            meshData = FaceDataTop(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x, y - 1, z).IsSolid(eFace.Top))
        {
            meshData = FaceDataBottom(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x, y, z + 1).IsSolid(eFace.Front))
        {
            meshData = FaceDataBack(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x, y, z - 1).IsSolid(eFace.Back))
        {
            meshData = FaceDataFront(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x + 1, y, z).IsSolid(eFace.Left))
        {
            meshData = FaceDataRight(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x - 1, y, z).IsSolid(eFace.Right))
        {
            meshData = FaceDataLeft(chunk, x, y, z, meshData);
        }

        return meshData;

    }

    protected virtual MeshData FaceDataTop (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(eFace.Top));
        return meshData;
    }

    protected virtual MeshData FaceDataBottom (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(eFace.Bottom));

        return meshData;
    }

    protected virtual MeshData FaceDataBack (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(eFace.Back));

        return meshData;
    }

    protected virtual MeshData FaceDataRight (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(eFace.Right));

        return meshData;
    }

    protected virtual MeshData FaceDataFront (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(eFace.Front));

        return meshData;
    }

    protected virtual MeshData FaceDataLeft (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(eFace.Left));

        return meshData;
    }
}
