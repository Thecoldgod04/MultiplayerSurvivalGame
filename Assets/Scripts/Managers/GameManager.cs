using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    private GameObject playerPrefab;

    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);


        if (PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.Joined)
            Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        else
            PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
    }
}
