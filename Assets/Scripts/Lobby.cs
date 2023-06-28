using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class Lobby : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TMP_InputField worldNameTextField;

    private void Start()
    {
        worldNameTextField.text = "Room1";

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void CreateWorld()   //After creating the room, the player will join the created room automatically
    {
        string roomName = worldNameTextField.text;
        PhotonNetwork.CreateRoom(roomName);
    }

    public void JoinWorld()
    {
        string roomName = worldNameTextField.text;
        PhotonNetwork.JoinRoom(roomName);
    }

    public void DeleteWorld()
    {

    }

    public void BackToMenu()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel("MainMenu");
    }


    //---------CALL BACKS---------//

    public override void OnConnectedToMaster()
    {
        Debug.Log(transform.name + ": Connected to master");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log(transform.name + ": Joined lobby");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log(transform.name + ": Created room");
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Ingame");
        Debug.Log(transform.name + ": Joined room");
    }
}
