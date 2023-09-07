using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BiomeLayer : TilemapLayer
{
    public const string sand = "SAND";
    public const string grass = "GRASS";
    public const string snow = "SNOW";
    public const string water = "WATER";

    [SerializeField]
    private TileBase[] biomeTiles;

    [SerializeField]
    private MapGenerator mapGenerator;

    public void GenerateBiomeBlock(string biome, Vector3 pos)
    {
        TileBase biomeTile = null;

        if(biome == sand)
        {
            biomeTile = biomeTiles[0];
        }
        else if(biome == grass)
        {
            biomeTile = biomeTiles[1];
        }
        else if (biome == snow)
        {
            biomeTile = biomeTiles[2];
        }
        else if (biome == water)
        {
            biomeTile = biomeTiles[3];
        }
        else
        {
            biomeTile = biomeTiles[0];
        }

        Vector3Int cellCoords = tilemap.WorldToCell(pos);
        tilemap.SetTile(cellCoords, biomeTile);
    }

    public void GenerateBiome(Vector3 pos)
    {
        int[,] biomeData = mapGenerator.biomeData;
        int x = (int) pos.x;
        int y = (int) pos.y;
        x = GetValidate(x); y = GetValidate(y);

        BiomeConfig biomeConfig = WorldConfiguration.instance.BiomeConfig;
        if(biomeData[x,y] == biomeConfig.ocean)
        {
            GenerateBiomeBlock("WATER", pos);
        }
        else if(biomeData[x, y] == biomeConfig.plains)
        {
            GenerateBiomeBlock("GRASS", pos);
        }
        else if (biomeData[x, y] == biomeConfig.snowy)
        {
            GenerateBiomeBlock("SNOW", pos);
        }
        else if (biomeData[x, y] == biomeConfig.desert)
        {
            GenerateBiomeBlock("SAND", pos);
        }
        else if (biomeData[x, y] == biomeConfig.beach)
        {
            GenerateBiomeBlock("SAND", pos);
        }
        else if (biomeData[x, y] == biomeConfig.jungle)
        {
            GenerateBiomeBlock("GRASS", pos);
        }
    }

    private int GetValidate(int i)
    {
        if (i < 0)
        {
            return 0;
        }
        else
            return i;
    }

    public void Refresh()
    {
        tilemap.RefreshAllTiles();
    }
}
