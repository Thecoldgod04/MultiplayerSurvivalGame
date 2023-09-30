using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using System.IO;
using Newtonsoft.Json;

public class MapSaveLoad : MonoBehaviourPun, ISaveable
{
    public static MapSaveLoad instance;

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

            string mapDataJson = saveLoadService.ReadFile(relativePath, encrypted);

            if (PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.Joined)
            {
                LoadMap(mapDataJson);
            }
            else
            {
                photonView.RPC("LoadMap", RpcTarget.AllBuffered, mapDataJson);
            }

            return true;
        }
        catch (FileNotFoundException e)
        {
            return false;
        }
    }

    [PunRPC]
    private void LoadMap(string mapDataJson)
    {
        MapData mapData = JsonConvert.DeserializeObject<MapData>(mapDataJson);

        //Load map generator data
        MapGenerator.instance.generated = true;

        MapGenerator.instance.SetSeed(mapData.seed);
    }

    public void LoadRPC()
    {
        Load(fileName);
    }

    public void Save(string fileName)
    {
        if (PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Joined && !PhotonNetwork.IsMasterClient) return;

        //string fileName = fileName;

        if (!saveLoadService.SaveData(folderName, fileName, GetMapData(), encrypted))
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

    private MapData GetMapData()
    {
        //seed
        int seed = MapGenerator.instance.GetSeed();

        return new MapData(seed);
    }
}

public class MapData
{
    public int seed;

    public MapData(int seed)
    {
        this.seed = seed;
    }
}
