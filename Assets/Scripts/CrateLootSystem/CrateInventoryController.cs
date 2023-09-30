using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using Photon.Pun;

public class CrateInventoryController : InventoryController
{
    [SerializeField]
    private CrateInventory crateInventory;

    [SerializeField]
    private GameObject UICrateInventory;

    [SerializeField]
    private Image[] iconList;

    [SerializeField]
    private TextMeshProUGUI[] amountTextList;

    [SerializeField]
    private List<Button> slotButtonList;

    private InventoryController playerInventoryController;

    // Start is called before the first frame update
    protected override void Start()
    {
        //RegisterAllUIElements();

        inventory = crateInventory;

        playerInventoryController = PlayerInventoryController.instance;

        UpdateUI();

        //RegisterOnClickFunction();
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateUI();
    }

    private void OnEnable()
    {
        //RegisterToPlayerInventoryController();
    }

    private void OnDisable()
    {
        UnregisterToPlayerInventoryController();
    }

    public void RegisterToPlayerInventoryController()
    {
        PlayerInventoryController.instance.SetCurrentOpenedInventory(this);   
    }
    public void UnregisterToPlayerInventoryController()
    {
        PlayerInventoryController.instance.SetCurrentOpenedInventory(null);
    }

    public void RegisterOnClickFunction()
    {
        for (int i = 0; i < slotButtonList.Count; i++)
        {
            Button button = slotButtonList[i];
            button.onClick.AddListener(new UnityAction(() => OnSlotButtonClicked(i)));
        }
    }

    public void OnSlotButtonClicked(int slotIndex)
    {
        if (playerInventoryController == null)
        {
            Debug.LogError("playerInventoryController == null");
            return;
        }

        DataBaseItemStack dataBaseItemStack = null;
        if (slotIndex + 1 <= crateInventory.GetItemList().Count)
        {
            dataBaseItemStack = crateInventory.GetItemList()[slotIndex];

            playerInventoryController.AddToInventory(dataBaseItemStack.itemMeta);

            this.RemoveFromInventory(dataBaseItemStack.itemMeta);

            /*int itemMetaId = ItemMetaManager.instance.itemMetaList.IndexOf(dataBaseItemStack.itemMeta);

            if (itemMetaId >= 0)
            {
                playerInventory.Add(dataBaseItemStack.itemMeta);

                if (PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.Joined)
                {
                    crateInventory.RemoveRPC(itemMetaId);
                }
                else if (crateInventory.GetPhotonView() != null)
                {
                    crateInventory.GetPhotonView().RPC("RemoveRPC", RpcTarget.AllBuffered, itemMetaId);
                }

                //crateInventory.Remove(dataBaseItemStack.itemMeta);
                UpdateCrateInventoryUI();
            }*/
        }
    }

    [PunRPC]
    public override void UpdateUI()
    {
        /*for(int i = 0; i < crateInventory.ItemList.Count; i++)
        {
            DataBaseItemStack tempDB = crateInventory.ItemList[i];

            iconList[i].sprite = tempDB.itemMeta.GetSprite();
            iconList[i].color = Color.white;
            iconList[i].color = new Color(1f, 1f, 1f, 1f);

            amountTextList[i].text = tempDB.amount.ToString();
        }*/

        if(inventory == null)
        {
            inventory = crateInventory;
        }

        for (int i = 0; i < slotButtonList.Count; i++)
        {
            if (i >= inventory.GetItemList().Count)
            {
                iconList[i].sprite = null;
                iconList[i].color = Color.white;
                iconList[i].color = new Color(1f, 1f, 1f, 0f);

                amountTextList[i].text = "";
            }
            else
            {
                DataBaseItemStack tempDB = inventory.GetItemList()[i];

                iconList[i].sprite = tempDB.itemMeta.GetSprite();
                iconList[i].color = Color.white;
                iconList[i].color = new Color(1f, 1f, 1f, 1f);

                amountTextList[i].text = tempDB.amount.ToString();
            }
        }
    }
}
