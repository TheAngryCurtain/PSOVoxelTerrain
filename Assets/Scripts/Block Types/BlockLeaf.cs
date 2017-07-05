using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class BlockLeaf : Block
{
    public BlockLeaf() : base()
    {

    }

    public override Tile TexturePosition(eFace face)
    {
        Tile tile = new Tile();
        tile.x = 0;
        tile.y = 1;

        return tile;
    }

    public override bool IsSolid(eFace face)
    {
        return false; // ensures that the blocks behind leaves are still rendered
    }
}