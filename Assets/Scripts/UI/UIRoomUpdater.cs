using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using TMPro;

public class UIRoomUpdater : MonoBehaviourPunCallbacks
{
    public static UIRoomUpdater instance;

    [SerializeField]
    private Transform uiRoomContainer;

    [SerializeField]
    private UIRoom uiRoomPrefab;

    [SerializeField]
    private TMP_InputField roomNameTextField;

    [Header("Do not edit!")]
    [SerializeField]
    private List<UIRoom> uiRoomList;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }


    private void AddToRoomList(RoomInfo roomInfo)
    {
        UIRoom room = Instantiate(uiRoomPrefab);
        room.SetRoomName(roomInfo.Name);
        room.SetplayerInRoom(roomInfo.PlayerCount);
        room.transform.SetParent(uiRoomContainer);
        room.transform.localScale = transform.root.localScale;

        if (!uiRoomList.Contains(room)) uiRoomList.Add(room);
    }

    private void RemoveFromRoomList(RoomInfo roomInfo)
    {
        UIRoom room = GetRoomByName(roomInfo.Name);

        foreach (Transform child in uiRoomContainer)
        {
            if (child.GetComponent<UIRoom>() != null && child.GetComponent<UIRoom>().roomName == room.roomName)
            {
                //Debug.LogWarning(child.GetComponent<UIRoom>().roomName);
                Destroy(child.gameObject);
            }
        }

        if (uiRoomList.Contains(room)) uiRoomList.Remove(room);
    }
    private UIRoom GetRoomByName(string name)
    {
        foreach (UIRoom room in uiRoomList)
        {
            if (room.roomName == name) return room;
        }
        return null;
    }

    public void SetRoomNameTextField(string name)
    {
        roomNameTextField.text = name;
    }

    //-------------CALL BACKS-------------

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList)    //The updated room is removed
                RemoveFromRoomList(roomInfo);
            else if (!uiRoomList.Contains(GetRoomByName(roomInfo.Name)))   //The updated room is not removed (*note: "is not removed" doesn't mean "is added"") 
            {
                //In this case, the room IS NOT REMOVED and it's NOT IN THE ROOMLIST => this room is added
                AddToRoomList(roomInfo);
            }
        }
        Debug.Log("OnRoomListUpdate: " + roomList.Count + " updated");
    }
}
