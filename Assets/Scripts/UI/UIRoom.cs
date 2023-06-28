using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIRoom : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI roomNameText;
    [SerializeField]
    private TextMeshProUGUI playerInRoomText;

    public string roomName { get; private set; }
    public int playerInRoom { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        roomNameText.text = roomName;
        playerInRoomText.text = playerInRoom.ToString() + "/4";
    }

    private void Update()
    {
        //Debug.LogError(roomName);
    }

    public void SetRoomName(string name)
    {
        roomName = name;
    }

    public void SetplayerInRoom(int amount)
    {
        playerInRoom = amount;
    }

    public void OnSelected()
    {
        UIRoomUpdater.instance.SetRoomNameTextField(roomName);
    }
}
