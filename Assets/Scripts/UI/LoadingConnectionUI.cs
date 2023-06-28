using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LoadingConnectionUI : MonoBehaviourPunCallbacks
{
    public override void OnJoinedLobby()
    {
        this.gameObject.SetActive(false);
    }
}
