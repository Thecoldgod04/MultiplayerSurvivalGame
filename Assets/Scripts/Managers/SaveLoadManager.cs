using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Photon.Pun;

public class SaveLoadManager : MonoBehaviourPun
{
    public static SaveLoadManager instance;

    private List<ISaveable> SaveableList = new List<ISaveable>();

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;

            SaveableList = GetComponentsInChildren<ISaveable>().ToList();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SaveGame()
    {
        for(int i = 0; i < SaveableList.Count; i++)
        {
            ISaveable saveable = SaveableList[i];

            saveable.SaveRPC();
        }
    }

    int requests = 0;
    public void LoadGameRPC()
    {
        requests++;

        if(requests >= 2)
        {
            LoadGame();
            /*if (PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.Joined)
            {
                //Load(fileName);
                LoadGame();
            }
            else
            {
                photonView.RPC("LoadGame", RpcTarget.AllBuffered);
            }*/
        }
    }

    //[PunRPC]
    public void LoadGame()
    {
        if (PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Joined && !PhotonNetwork.IsMasterClient) return;

        for (int i = 0; i < SaveableList.Count; i++)
        {
            ISaveable saveable = SaveableList[i];

            if (saveable.IsLoadedByManager == true)
            {
                //saveable.Load(saveable.fileName);
                saveable.LoadRPC();
            }
        }
    }
}
