using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using UnityEngine.Events;

public class PlayerInventoryController : InventoryController
{
    public static PlayerInventoryController instance;

    [Header("UI Elements")]
    [SerializeField]
    private List<Image> iconList;

    [SerializeField]
    private List<TextMeshProUGUI> amountTextList;

    [SerializeField]
    private List<Button> slotButtonList;

    [SerializeField]
    private Transform selectionFrame;

    public static ItemMeta itemInHand { get; private set; }

    [Header("Controller")]
    [SerializeField]
    private int currentSelection;

    [field: SerializeField]
    public InventoryController CurrentOpenedInventory { get; private set; }

    //[Header("Database")]
    [field: SerializeField]
    public Inventory inventoryDatabase { get; private set; }

    protected override void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        inventory = inventoryDatabase;

        if (iconList.Count != amountTextList.Count)
        {
            Debug.LogError(transform.name + ": iconList.Count or amountTextList.Count or inventoryDatabase.GetItemList().Count does not equal!");
            return;
        }

        //UpdateInventoryUI();
        UpdateUI();
    }

    public void UpdateInventoryUI()
    {
        int i = 0;
        foreach(DataBaseItemStack db in inventory.GetItemList())
        {
            iconList[i].sprite = db.itemMeta.GetSprite(); iconList[i].color = Color.white;
            amountTextList[i].text = db.amount.ToString();
            i++;
        }
    }

    public void SetCurrentOpenedInventory(InventoryController inventoryController)
    {
        CurrentOpenedInventory = inventoryController;
    }

    public void RegisterOnClickFunction()
    {
        for(int i = 0; i < slotButtonList.Count; i++)
        {
            Button button = slotButtonList[i];
            button.onClick.AddListener(new UnityAction(() => OnSlotButtonClicked(i)));
        }
    }

    public void OnSlotButtonClicked(int slotIndex)
    {
        if (CurrentOpenedInventory == null)
        {
            //Debug.LogError("CurrentOpenedInventory == null");
            return;
        }

        DataBaseItemStack dataBaseItemStack = null;
        if(slotIndex+1 <= inventory.GetItemList().Count)
        {
            dataBaseItemStack = inventory.GetItemList()[slotIndex];

            CurrentOpenedInventory.AddToInventory(dataBaseItemStack.itemMeta);

            this.RemoveFromInventory(dataBaseItemStack.itemMeta);

            /*int itemMetaId = ItemMetaManager.instance.itemMetaList.IndexOf(dataBaseItemStack.itemMeta);

            if (itemMetaId >= 0)
            {
                if (PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.Joined)
                {
                    CurrentOpenedInventory.AddRPC(itemMetaId);
                }
                else if (CurrentOpenedInventory.GetPhotonView() != null)
                {
                    CurrentOpenedInventory.GetPhotonView().RPC("AddRPC", RpcTarget.AllBuffered, itemMetaId);
                }

                inventory.Remove(dataBaseItemStack.itemMeta);
                UpdateUI();
            }*/
        }
    }

    public void UpdateSelection()
    {
        //if (photonView.IsMine == false) return;

        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");

        if (scrollDelta > 0f)   //scroll up
        {
            currentSelection--;
            if (currentSelection < 0)
                currentSelection = 6;
        }
        else if(scrollDelta < 0f) //scroll down
        {
            currentSelection++;
            if (currentSelection > 6)
                currentSelection = 0;
        }

        selectionFrame.position = iconList[currentSelection].transform.position;

        if (inventory.GetItemList().Count == 0 ||
            inventory.GetItemList().Count-1 < currentSelection ||
            inventory.GetItemList()[currentSelection] == null)
        {
            itemInHand = null;
            return;
        }
        itemInHand = inventory.GetItemList()[currentSelection].itemMeta;
    }

    private void Update()
    {
        UpdateSelection();
        //UpdateUI();
    }

    /*public ov void AddToInventory(ItemMeta itemMeta)
    {
        inventoryDatabase.Add(itemMeta);
        UpdateUI();
    }*/

    public override void UpdateUI()
    {
        /*if(inventory.GetItemList().Count == 0)
        {
            for (int i = 0; i < amountTextList.Count; i++)
            {
                iconList[i].sprite = null;
                iconList[i].color = Color.white;
                iconList[i].color = new Color(1f, 1f, 1f, 0f);

                amountTextList[i].text = "";
            }
        }
        for (int i = 0; i < inventory.GetItemList().Count; i++)
        {
            DataBaseItemStack tempDB = inventory.GetItemList()[i];

            iconList[i].sprite = tempDB.itemMeta.GetSprite();
            iconList[i].color = Color.white;
            iconList[i].color = new Color(1f, 1f, 1f, 1f);

            amountTextList[i].text = tempDB.amount.ToString();
        }*/

        for(int i = 0; i < slotButtonList.Count; i++)
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

    /*public IInventory GetInventory()
    {
        return inventory;
    }*/

    //Event handling

    public bool OnItemCollected(ItemStack itemStack)
    {
        //if (photonView.IsMine == false) return;
        //DatabaseItemStack databaseItemStack = new DatabaseItemStack { itemStack = itemStack };

        //Update database
        int indexOfAddedItem = inventory.Add(itemStack);

        if (indexOfAddedItem == -1) return false;

        //Update UI
        /*iconList[indexOfAddedItem].sprite = inventoryDatabase.Get(indexOfAddedItem).itemMeta.GetSprite();
        iconList[indexOfAddedItem].color = Color.white;
        iconList[indexOfAddedItem].color = new Color(1f, 1f, 1f, 1f);

        amountTextList[indexOfAddedItem].text = inventoryDatabase.Get(indexOfAddedItem).amount.ToString();*/
        UpdateUI();

        return true;
    }
}
