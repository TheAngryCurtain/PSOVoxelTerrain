using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class BlockGrass : Block
{
    public BlockGrass() : base()
    {

    }

    public override Tile TexturePosition(eFace face)
    {
        Tile tile = new Tile();
        switch (face)
        {
            case eFace.Top:
                tile.x = 2;
                tile.y = 0;
                return tile;

            case eFace.Bottom:
                tile.x = 1;
                tile.y = 0;
                return tile;
        }

        tile.x = 3;
        tile.y = 0;
        return tile;
    }
}
