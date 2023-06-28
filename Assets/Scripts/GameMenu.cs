using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameMenu : MonoBehaviour
{
    public void Option()
    {

    }

    public void Leave()
    {
        PhotonNetwork.LoadLevel("MainMenu");
        PhotonNetwork.LeaveRoom();
    }
}
