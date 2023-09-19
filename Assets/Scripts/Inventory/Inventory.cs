using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory")]
public class Inventory : ScriptableObject, IInventory
{
    //The first 7 slots are the toolbar slots
    //[SerializeField]
    //private Dictionary<ItemMeta, int> itemList = new Dictionary<ItemMeta, int>();

    [SerializeField]
    private List<DataBaseItemStack> itemList = new List<DataBaseItemStack>();

    [SerializeField]
    private int inventorySize;

    public void SetInventory(List<DataBaseItemStack> itemList)
    {
        this.itemList = itemList;
    }

    public int Add(ItemStack itemStack)
    {
        if (IsFull()) return -1;

        int index = -1;

        ItemMeta itemMeta = itemStack.GetItemMeta();
        int amount = itemStack.GetAmount();
        if(GetAvailable(itemMeta) != null)
        {
            index = GetIndex(GetAvailable(itemMeta));
            GetAvailable(itemMeta).amount++;
        }
        else
        {
            DataBaseItemStack newDb = new DataBaseItemStack
            {
                itemMeta = itemMeta,
                amount = amount
            };
            itemList.Add(newDb);
            index = GetIndex(newDb);
        }

        return index;
    }

    public int Add(ItemMeta itemMeta)
    {
        if (IsFull()) return -1;

        int index = -1;
        if (GetAvailable(itemMeta) != null)
        {
            index = GetIndex(GetAvailable(itemMeta));
            GetAvailable(itemMeta).amount++;
        }
        else
        {
            DataBaseItemStack newDb = new DataBaseItemStack
            {
                itemMeta = itemMeta,
                amount = 1
            };
            itemList.Add(newDb);
            index = GetIndex(newDb);
            //Debug.LogError(ItemList.Count);
        }
        return index;
    }

    public int AddRPC(int itemMetaId)
    {
        ItemMeta itemMeta = ItemMetaManager.instance.GetItemMetaById(itemMetaId);
        return Add(itemMeta);
    }

    public int Remove(ItemMeta itemMeta)
    {
        int index = -1;

        if (GetAvailable(itemMeta) != null)
        {
            DataBaseItemStack available = GetAvailable(itemMeta);
            index = GetIndex(available);

            if(available.amount == 1)
            {
                itemList.Remove(available);
            }
            else
            {
                GetAvailable(itemMeta).amount--;
            }
        }

        return index;
    }

    public int RemoveRPC(int itemMetaId)
    {
        ItemMeta itemMeta = ItemMetaManager.instance.GetItemMetaById(itemMetaId);
        return Remove(itemMeta);
    }

    public PhotonView GetPhotonView()
    {
        return null;
    }

    public bool Contains(ItemMeta itemMeta)
    {
        foreach(DataBaseItemStack db in itemList)
        {
            if (db.itemMeta == itemMeta)
                return true;
        }
        return false;
    }

    public int GetIndex(DataBaseItemStack db)
    {
        return itemList.IndexOf(db);
    }

    public DataBaseItemStack Get(int index)
    {
        return itemList[index];
    }

    public List<DataBaseItemStack> GetItemList()
    {
        return itemList;
    }

    public DataBaseItemStack GetAvailable(ItemMeta itemMeta)
    {
        foreach(DataBaseItemStack db in itemList)
        {
            if (db.itemMeta == itemMeta &&
                db.amount < db.itemMeta.GetMaxAmount())
                return db;
        }
        return null;
    }

    public bool IsFull()
    {
        return itemList.Count >= inventorySize;
    }
}

[System.Serializable]
public class DataBaseItemStack
{
    public ItemMeta itemMeta;
    public int amount;
}
