using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using System.IO;

public class PlayerSaveLoad : MonoBehaviourPun
{
    public static PlayerSaveLoad instance;

    [SerializeField] private string folderName;

    private string fileName;

    //private string relativePath;

    IDataService saveLoadService = new SaveLoadService();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        if (PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.Joined)
        {
            fileName = "TestClient";
        }
        else
        {
            fileName = PhotonNetwork.LocalPlayer.NickName;
            //Debug.LogError(fileName);
        }
        //Debug.LogError(fileName);
        
        //relativePath = "/" + folderName + "/" + fileName + ".json";
    }

    private void Start()
    {
        //fileName = GameManager.instance.PlayerOwner.GetPhotonView().ViewID + ".json";
    }

    public void SavePlayerRPC()
    {
        if(PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.Joined)
        {
            SavePlayer(fileName);
        }
        else
        {
            //SavePlayer(fileName, GameManager.instance.PlayerOwner.GetPhotonView().ViewID);
            photonView.RPC("SavePlayer", RpcTarget.MasterClient, fileName);
        }
    }

    
    public void LoadPlayerRPC()
    {
        //string fileName = player.photonView.Owner.NickName;

        //Debug.LogError(fileName);

        if (PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.Joined)
        {
            LoadPlayer(fileName);
        }
        else
        {
            //SavePlayer(fileName, GameManager.instance.PlayerOwner.GetPhotonView().ViewID);
            photonView.RPC("LoadPlayer", RpcTarget.MasterClient, fileName);
        }
    }

    [PunRPC]
    public void SavePlayer(string playerName)
    {
        if (PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Joined && !PhotonNetwork.IsMasterClient) return;

        string fileName = playerName;

        GameObject player = null;

        if (PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Joined)
        {
            foreach (GameObject p in GameManager.instance.PlayerList)
            {
                if (p.GetPhotonView().Owner.NickName == playerName)
                {
                    player = p;
                    break;
                }
            }
        }
        else
        {
            player = GameManager.instance.PlayerOwner;
        }
        
        if(player != null)
        {
            if (!saveLoadService.SaveData(folderName, fileName, GetPlayerData(player), false))
            {
                Debug.LogError("Did not save player due to some issues!");
            }
            else
            {
                GameManager.instance.PlayerList.Remove(player);
            }
        }
        else
        {
            Debug.LogError("Can not find player with name: " + fileName + " to save!");
        }
    }

    [PunRPC]
    public bool LoadPlayer(string playerName)
    {
        if (PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Joined && !PhotonNetwork.IsMasterClient) return false;

        GameObject player = null;

        //Debug.LogError("Loading for player named: " + playerName);

        if (PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Joined)
        {
            //Debug.LogError(GameManager.instance.PlayerList.Count);

            //foreach()

            foreach (PhotonView pv in FindObjectsOfType<PhotonView>())
            {
                if (pv.Owner.NickName == playerName && pv.GetComponent<PlayerSetup>() != null)
                {
                    player = pv.gameObject;
                    break;
                }
            }
        }
        else
        {
            player = GameManager.instance.PlayerOwner;
        }

        if(player != null)
        {
            try
            {
                string relativePath = "/" + folderName + "/" + playerName + ".json";

                //PlayerData playerData = saveLoadService.LoadData<PlayerData>(relativePath, false);
                string playerData = saveLoadService.ReadFile(relativePath, false);

                //Debug.LogError("Loading for player named: " + playerName);

                player.GetComponent<PlayerSetup>().ApplyLoadedDataRPC(playerData);

                return true;
            }
            catch (FileNotFoundException e)
            {
                player.GetComponent<PlayerSetup>().FinishSetup();
                Debug.LogError("This might be a new player!");
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public PlayerData GetPlayerData(GameObject player)
    {
        //GameObject player = GameManager.instance.PlayerOwner;

        string name = "";
        if (PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.Joined)
        {
            name = "TestClient";
        }
        else
        {
            name = player.GetPhotonView().Owner.NickName;
        }

        float[] position = { player.transform.position.x, player.transform.position.y, player.transform.position.z };

        int health = player.GetComponent<Life>().health;

        Inventory inventoryDB = PlayerInventoryController.instance.inventoryDatabase;
        Dictionary<int, int[]> inventory = new Dictionary<int, int[]>();
        for (int i = 0; i < inventoryDB.GetItemList().Count; i++)
        {
            int itemMetaId = ItemMetaManager.instance.itemMetaList.IndexOf(inventoryDB.GetItemList()[i].itemMeta);
            int amount = inventoryDB.GetItemList()[i].amount;

            int[] itemData = { itemMetaId, amount };

            inventory[i] = itemData;
            //inventory.Add(new SerializableInventoryData(i, itemData));
        }

        string savedBy = "";

        if(PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.Joined)
        {
            savedBy = "TestClient";
        }
        else
        {
            savedBy = GameManager.instance.PlayerOwner.GetPhotonView().Owner.NickName;
        }

        return new PlayerData(name, position, health, inventory, savedBy);
    }
}

public class PlayerData
{
    public string name;

    public float[] position;

    public int health;

    public Dictionary<int, int[]> inventory;

    //public List<SerializableInventoryData> inventory;

    public string savedBy;

    public PlayerData(string name, float[] position, int health, Dictionary<int, int[]> inventory, string savedBy)
    {
        this.name = name;
        this.position = position;
        this.health = health;
        this.inventory = inventory;
        this.savedBy = savedBy;
    }
}


public class SerializableInventoryData
{
    public int key;
    public int[] data;

    public SerializableInventoryData(int key, int[] data)
    {
        this.key = key;
        this.data = data;
    }
}
