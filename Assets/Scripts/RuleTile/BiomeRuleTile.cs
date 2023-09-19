using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

[CreateAssetMenu(menuName = "RuleTiles/Biome Rule Tile")]
public class BiomeRuleTile : RuleTile<BiomeRuleTile.Neighbor> {
    [Header("Advanced Tile")]
    [Tooltip("If enabled, the tile will connect to these tiles too when the mode is set to \"This\"")]
    public bool alwaysConnect;
    [Tooltip("Tiles to connect to")]
    public TileBase[] tilesToConnect3;
    [Space]
    [Tooltip("Tiles to connect to")]
    public TileBase[] tilesToConnect4;
    [Space]
    [Tooltip("Tiles to connect to")]
    public TileBase[] tilesToConnect5;
    [Space]
    [Tooltip("Check itseft when the mode is set to \"any\"")]
    public bool checkSelf = true;

    public class Neighbor : RuleTile.TilingRule.Neighbor
    {
        public const int Specified3 = 3;
        public const int Specified4 = 4;
        public const int Specified5 = 5;
    }

    public override bool RuleMatch(int neighbor, TileBase tile)
    {
        switch (neighbor)
        {
            case Neighbor.This: return Check_This(tile);
            case Neighbor.NotThis: return Check_NotThis(tile);
            case Neighbor.Specified3: return Check_Specified3(tile);
            case Neighbor.Specified4: return Check_Specified4(tile);
            case Neighbor.Specified5: return Check_Specified5(tile);
        }
        return base.RuleMatch(neighbor, tile);
    }
    bool Check_This(TileBase tile)
    {
        if (!alwaysConnect) return tile == this;
        else return (tilesToConnect3.Contains(tile) || tilesToConnect4.Contains(tile) || tilesToConnect5.Contains(tile)) || tile == this;
        //.Contains requires "using System.Linq;"
    }
    bool Check_NotThis(TileBase tile)
    {
        if (!alwaysConnect) return tile != this;
        else return !tilesToConnect3.Contains(tile) && !tilesToConnect4.Contains(tile) && !tilesToConnect5.Contains(tile) && tile != this;
        //.contains requires "using system.linq;"
    }
    bool Check_Specified3(TileBase tile)
    {
        if (checkSelf) return tile != null;
        else return tile != null && tile != this && tile == tilesToConnect3.Contains(tile);
    }
    bool Check_Specified4(TileBase tile)
    {
        if (checkSelf) return tile != null;
        else return tile != null && tile != this && tile == tilesToConnect4.Contains(tile);
    }
    bool Check_Specified5(TileBase tile)
    {
        if (checkSelf) return tile != null;
        else return tile != null && tile != this && tile == tilesToConnect5.Contains(tile);
    }
}