using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class BlockWood : Block
{
    public BlockWood() : base()
    {

    }

    public override Tile TexturePosition(eFace face)
    {
        Tile tile = new Tile();
        switch (face)
        {
            case eFace.Top:
                tile.x = 2;
                tile.y = 1;
                return tile;

            case eFace.Bottom:
                tile.x = 2;
                tile.y = 1;
                return tile;
        }

        tile.x = 1;
        tile.y = 1;

        return tile;
    }
}