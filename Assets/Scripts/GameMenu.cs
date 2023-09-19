using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviourPunCallbacks
{
    public void Option()
    {

    }

    public void Leave()
    {
        if(PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.Joined)
        {
            Debug.LogError("Quit Game");
        }
        else
        {
            Debug.LogError("Quit Multiplayer Game");
            //PhotonNetwork.LeaveRoom(true);
        }
    }
    public override void OnLeftRoom()
    {
        StartCoroutine(WaitToLeave());
    }

    IEnumerator WaitToLeave()
    {
        while (PhotonNetwork.InRoom)
            yield return null;
        SceneManager.LoadScene("MainMenu");
    }
}
