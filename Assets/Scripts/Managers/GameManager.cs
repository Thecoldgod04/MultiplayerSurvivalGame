using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;

    [SerializeField]
    private GameObject playerPrefab;

    [field: SerializeField]
    public GameObject PlayerOwner { get; private set; }    //this is the owner of the player character NOT the master client
    
    [field: SerializeField]
    public List<GameObject> PlayerList { get; private set; }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            if (PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.Joined)
            {
                PlayerOwner = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            }
            else
            {
                PlayerOwner = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
