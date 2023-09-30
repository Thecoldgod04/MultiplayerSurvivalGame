using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using System.IO;
using Newtonsoft.Json;

public class WorldSaveLoad : MonoBehaviourPun, ISaveable
{
    public static WorldSaveLoad instance;

    [field: SerializeField]
    public string folderName { get; private set; }

    [field: SerializeField]
    public bool encrypted { get; private set; }

    [field: SerializeField]
    public string fileName { get; private set; }

    public IDataService saveLoadService { get; private set; }

    [field: SerializeField]
    public bool IsLoadedByManager { get; private set; }

    public bool Load(string fileName)
    {
        if (PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Joined && !PhotonNetwork.IsMasterClient) return false;

        try
        {
            string relativePath = "/" + folderName + "/" + fileName + ".json";

            string worldDataJson = saveLoadService.ReadFile(relativePath, encrypted);

            if(PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.Joined)
            {
                LoadWorld(worldDataJson);
                LoadWorldTime(worldDataJson);
            }
            else
            {
                photonView.RPC("LoadWorld", RpcTarget.AllBuffered, worldDataJson);
                photonView.RPC("LoadWorldTime", RpcTarget.AllBuffered, worldDataJson);
            }

            return true;
        }
        catch (FileNotFoundException e)
        {
            return false;
        }
    }

    //int requests = 0;
    public void LoadRPC()
    {
        Load(fileName);
    }

    [PunRPC]
    private void LoadWorld(string worldDataJson)
    {
        WorldData worldData = JsonConvert.DeserializeObject<WorldData>(worldDataJson);

        //Load chunks
        for (int i = 0; i < worldData.loadedChunks.Count; i++)
        {
            int[] chunkPos = worldData.loadedChunks[i];
            Vector2 pos = new Vector2(chunkPos[0], chunkPos[1]);

            GameObject newChunkObject = new GameObject("" + pos);
            newChunkObject.transform.SetParent(ChunkManager.instance.transform);

            ChunkManager.instance.LoadedChunks.Add(pos, newChunkObject);
        }

        for (int i = 0; i < worldData.occupiedList.Count; i++)
        {
            Occupied occupied = worldData.occupiedList[i];
            Vector3 pos = new Vector3(occupied.pos[0], occupied.pos[1], occupied.pos[2]);
            GameObject buildObject = ConstructionLayer.instance.Build(pos, (BuildableMeta)ItemMetaManager.instance.GetItemMetaById(occupied.buildableId));

            GameObject chunkObject = null;

            if (ChunkManager.instance.LoadedChunks.ContainsKey(ChunkManager.instance.GetChunkPosition(pos)))
            {
                chunkObject = ChunkManager.instance.LoadedChunks[ChunkManager.instance.GetChunkPosition(pos)];
            }

            if (chunkObject != null && buildObject != null)
            {
                buildObject.transform.SetParent(chunkObject.transform);
            }
        }
    }

    [PunRPC]
    private void LoadWorldTime(string worldDataJson)
    {
        WorldData worldData = JsonConvert.DeserializeObject<WorldData>(worldDataJson);

        WorldTime.instance.latestSavedTime = worldData.lastSavedTime;
    }

    public void Save(string fileName)
    {
        if (PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Joined && !PhotonNetwork.IsMasterClient) return;

        //string fileName = fileName;

        if (!saveLoadService.SaveData(folderName, fileName, GetWorldData(), encrypted))
        {
            Debug.LogError("Did not save player due to some issues!");
        }
        else
        {
            //GameManager.instance.PlayerList.Remove(player);
        }
    }

    public void SaveRPC()
    {
        Save(fileName);
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        saveLoadService = new SaveLoadService();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public WorldData GetWorldData()
    {
        //occupiedList
        List<Occupied> occupiedList = new List<Occupied>();

        foreach(KeyValuePair<Vector3Int, BuildableObject> occupied in ConstructionLayer.instance.occupiedList)
        {
            int[] key = { occupied.Key.x, occupied.Key.y, occupied.Key.z };
            //Vector3 key = occupied.Key;

            int value = ItemMetaManager.instance.itemMetaList.IndexOf(occupied.Value.buildableMeta);

            occupiedList.Add(
                new Occupied
                {
                    pos = key,
                    buildableId = value
                }    
            );
        }

        //loadedChunks
        List<int[]> loadedChunks = new List<int[]>();

        foreach(KeyValuePair<Vector2, GameObject> chunk in ChunkManager.instance.LoadedChunks)
        {
            int[] chunkPos = {(int)chunk.Key.x, (int)chunk.Key.y };
            loadedChunks.Add(chunkPos);
        }

        //currentTime
        float lastSavedTime = WorldTime.instance.GetCurrentTime();

        return new WorldData(occupiedList, loadedChunks, lastSavedTime);
    }
}

public class WorldData
{
    public List<Occupied> occupiedList = new List<Occupied>();

    public List<int[]> loadedChunks;

    public float lastSavedTime;

    public WorldData(List<Occupied> occupiedList, List<int[]> loadedChunks, float lastSavedTime)
    {
        this.occupiedList = occupiedList;
        this.loadedChunks = loadedChunks;
        this.lastSavedTime = lastSavedTime;
    }
}

public struct Occupied
{
    public int[] pos;
    public int buildableId;
}
