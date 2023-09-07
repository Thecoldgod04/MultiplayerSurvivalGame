using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnvironmentGenerator : MonoBehaviour
{
    [SerializeField]
    private MapGenerator mapGenerator;

    [SerializeField]
    private ConstructionLayer constructionLayer;

    //public static BuildableMeta cactus;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.LogWarning(ItemMetaManager.instance == null);
        //cactus = (BuildableMeta)ItemMetaManager.instance.itemMetaList[3];

        //GenerateEnv(mapGenerator.environmentData);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateEnvironment(Vector3 pos)
    {
        int[,] environmentData = mapGenerator.environmentData;
        /*for (int y = 0; y < environmentData.GetLength(0); y++)
        {
            for (int x = 0; x < environmentData.GetLength(0); x++)
            {
                if (environmentData[x, y] != 0)
                {
                    //Debug.LogWarning(cactus == null);
                    constructionLayer.Build(new Vector3(x, y), cactus);
                }
            }
        }*/
        int x = (int)pos.x;
        int y = (int)pos.y;
        x = GetValidate(x); y = GetValidate(y);

        if (environmentData[x, y] != 0)
        {
            //Debug.LogWarning(cactus == null);
            int envir = environmentData[x, y];
            constructionLayer.Build(new Vector3(x, y), (BuildableMeta)ItemMetaManager.instance.itemMetaList[envir]);
        }
    }

    public void LoadEnvironment(Vector3 pos)
    {
        constructionLayer.Load(pos);
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
    /*
        public void PlaceTile(TileBase tile, Vector3 pos)
        {
            Vector3Int cellCoords = tilemap.WorldToCell(pos);
            tilemap.SetTile(cellCoords, tile);
        }*/
}
