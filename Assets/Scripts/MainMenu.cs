using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MainMenu : MonoBehaviour
{
    //private bool singleplayer = false;

    private void Start()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.LocalPlayer.NickName = "Client";
    }

    public void PlaySingleplayer()      //This means the player wants to create a room for himself and may open the room for multiplayer later
    {
        PhotonNetwork.LoadLevel("SingleplayerLobby");
    }

    public void PlayMultiplayer()
    {
        PhotonNetwork.LoadLevel("MultiplayerLobby");
    }

    public void Option()
    {

    }

    public void QuitGame()
    {
        
    }
}
