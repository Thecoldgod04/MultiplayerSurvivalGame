using System.Collections;
using System.Collections.Generic;
using TMPro;
using Photon.Pun;
using Newtonsoft.Json;
using UnityEngine.Events;
using UnityEngine;

public class PlayerSetup : MonoBehaviourPun
{
    [SerializeField]
    private GameObject playerCamera;

    [SerializeField]
    private TextMeshProUGUI playerNameText;

    public UnityEvent onFinishSetup;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.PlayerList.Add(this.gameObject);

        if(photonView.Owner == null)
            playerNameText.text = "TestClient";
        else
            playerNameText.text = photonView.Owner.NickName;

        /*if(photonView.IsMine)
        {
            PlayerSaveLoad.instance.LoadPlayerRPC(this);
        }*/

        if (photonView.ViewID != 0 && !photonView.IsMine) return;

        PlayerSaveLoad.instance.LoadPlayerRPC();

        playerCamera.SetActive(true);
        playerCamera.tag = "MainCamera";
    }

    public void ApplyLoadedDataRPC(string playerDataJson)
    {
        //PlayerData playerData = JsonConvert.DeserializeObject<PlayerData>(playerDataJson);

        /*string name = playerData.name;

        float[] position = playerData.position;

        int health = playerData.health;

        List<SerializableInventoryData> inv = new List<SerializableInventoryData>();
        foreach (SerializableInventoryData itemData in playerData.inventory)
        {
            inv.Add(itemData);
        }

        object[] inventory = inv.ToArray();
        //object[] inventoryObj = (object[])inventory;*/

        if (PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.Joined)
        {
            ApplyLoadedData(playerDataJson);
        }
        else
        {
            photonView.RPC("ApplyLoadedData", RpcTarget.All, playerDataJson);
        }
    }

    [PunRPC]
    private void ApplyLoadedData(string playerDataJson)
    {
        if (PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Joined && !photonView.IsMine) return;

        PlayerData playerData = JsonConvert.DeserializeObject<PlayerData>(playerDataJson);

        //Debug.LogError("Loading for player named: " + photonView.Owner.NickName);

        transform.position = new Vector3(playerData.position[0], playerData.position[1], playerData.position[2]);
        GetComponent<Life>().SetHealth(playerData.health);

        List<DataBaseItemStack> inventory = new List<DataBaseItemStack>();
        foreach (KeyValuePair<int, int[]> itemData in playerData.inventory)
        {
            inventory.Add(new DataBaseItemStack
            {
                itemMeta = ItemMetaManager.instance.GetItemMetaById(itemData.Value[0]),
                amount = itemData.Value[1]
            });
        }
        PlayerInventoryController.instance.inventoryDatabase.SetInventory(inventory);

        PlayerInventoryController.instance.UpdateUI();
        PlayerHealthController.instance.UpdateHealthUI();

        FinishSetup();
    }

    public void FinishSetup()
    {
        onFinishSetup.Invoke();
    }
}
