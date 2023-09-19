using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Photon.Pun;

public class EnvironmentGenerator : MonoBehaviourPun
{
    [SerializeField]
    private MapGenerator mapGenerator;

    [SerializeField]
    private ConstructionLayer constructionLayer;

    private List<Vector3> loadedDataObjects;

    //public static BuildableMeta cactus;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.LogWarning(ItemMetaManager.instance == null);
        //cactus = (BuildableMeta)ItemMetaManager.instance.itemMetaList[3];

        //GenerateEnv(mapGenerator.environmentData);
        loadedDataObjects = ChunkManager.instance.LoadedDataObjects;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool calledForRequest = false;
    public GameObject GenerateEnvironment(Vector3 pos, bool containsDataObjects)
    {
        int[,] environmentData = mapGenerator.environmentData;

        //bool generateDataObjects = !ChunkManager.instance.IsInChunksOfLoadedDataObjects(pos);

        GameObject environmentObj = null;

        //Debug.LogWarning(environmentData[0, 57]);
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
            //Debug.LogWarning(environmentData[x, y]);

            BuildableMeta buildableMeta = (BuildableMeta)ItemMetaManager.instance.itemMetaList[envir];

            if(containsDataObjects && buildableMeta.GetBuildableType() == BuildableType.DataGameObject)
            {

            }
            else
            {
                environmentObj = constructionLayer.Build(new Vector3(x, y), buildableMeta);

                if(buildableMeta.GetBuildableType() == BuildableType.DataGameObject)
                {
                    //DataObject dataObject = environmentObj.GetComponent<DataObject>();

                    if (PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Joined)
                    {
                        int dataX = ItemMetaManager.instance.itemMetaList.IndexOf(buildableMeta);

                        //int dataY = DataObjectManager.instance.DataObjectList.IndexOf(dataObject);
                        int dataY = DataObject.staticId;

                        Vector2 data = new Vector2(dataX, dataY);

                        photonView.RPC("RequestRegisterBuildableObject", RpcTarget.AllBuffered, pos, data);
                    }
                }
            }
            //environmentObj = obj.gameObject;
        }

        return environmentObj;
    }

    [PunRPC]
    public void RequestRegisterBuildableObject(Vector3 pos, Vector2 data)
    {
        int dataX = (int)data.x;    //buildableMeta's Id
        int dataY = (int)data.y;    //dataObject's Id

        BuildableMeta buildableMeta = (BuildableMeta)ItemMetaManager.instance.itemMetaList[dataX];

        GameObject dataGameObject = null;

        foreach(DataObject dataObject in FindObjectsOfType<DataObject>())
        {
            if (dataObject.Id == data.y)
                dataGameObject = dataObject.gameObject;
        }

        BuildableObject buildableObject = null;

        if (dataGameObject != null && buildableMeta != null)
        {
            buildableObject = new BuildableObject(buildableMeta, dataGameObject);
        }

        constructionLayer.RegisterObjectToOccupiedList(pos, buildableObject);

        //Debug.LogError("Registered for object: " + dataGameObject.name);
    }

    public GameObject LoadEnvironment(Vector3 pos)
    {
        return constructionLayer.Load(pos);
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
