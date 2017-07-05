using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class BlockStone : Block
{
    public BlockStone() : base()
    {

    }

    public override Tile TexturePosition(eFace face)
    {
        Tile tile = new Tile();
        tile.x = 0;
        tile.y = 0;
        return tile;
    }
}
