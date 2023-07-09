using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class PlayerInventoryController : MonoBehaviourPun
{
    public static PlayerInventoryController instance;

    [Header("UI Elements")]
    [SerializeField]
    private List<Image> iconList;

    [SerializeField]
    private List<TextMeshProUGUI> amountTextList;

    [SerializeField]
    private Transform selectionFrame;

    public static ItemMeta itemInHand { get; private set; }

    [Header("Controller")]
    [SerializeField]
    private int currentSelection;

    [Header("Database")]
    [SerializeField]
    private Inventory inventoryDatabase;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        if (iconList.Count != amountTextList.Count)
        {
            Debug.LogError(transform.name + ": iconList.Count or amountTextList.Count or inventoryDatabase.GetItemList().Count does not equal!");
            return;
        }

        UpdateInventoryUI();
    }

    public void UpdateInventoryUI()
    {
        int i = 0;
        foreach(DataBaseItemStack db in inventoryDatabase.GetItemList())
        {
            iconList[i].sprite = db.itemMeta.GetSprite(); iconList[i].color = Color.white;
            amountTextList[i].text = db.amount.ToString();
            i++;
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

        if (inventoryDatabase.GetItemList().Count == 0 ||
            inventoryDatabase.GetItemList().Count-1 < currentSelection ||
            inventoryDatabase.GetItemList()[currentSelection] == null)
        {
            itemInHand = null;
            return;
        }
        itemInHand = inventoryDatabase.GetItemList()[currentSelection].itemMeta;
    }

    private void Update()
    {
        UpdateSelection();
    }

    //Event handling

    public void OnItemCollected(ItemStack itemStack)
    {
        //if (photonView.IsMine == false) return;
        //DatabaseItemStack databaseItemStack = new DatabaseItemStack { itemStack = itemStack };

        //Update database
        int indexOfAddedItem = inventoryDatabase.Add(itemStack);

        //Update UI
        iconList[indexOfAddedItem].sprite = inventoryDatabase.Get(indexOfAddedItem).itemMeta.GetSprite(); 
        iconList[indexOfAddedItem].color = Color.white;
        iconList[indexOfAddedItem].color = new Color(1f, 1f, 1f, 1f);
        
        amountTextList[indexOfAddedItem].text = inventoryDatabase.Get(indexOfAddedItem).amount.ToString();
    }
}
