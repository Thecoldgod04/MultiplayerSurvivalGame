using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory")]
public class Inventory : ScriptableObject
{
    //The first 7 slots are the toolbar slots
    //[SerializeField]
    //private Dictionary<ItemMeta, int> itemList = new Dictionary<ItemMeta, int>();

    [SerializeField]
    private List<DataBaseItemStack> itemList = new List<DataBaseItemStack>();

    public int Add(ItemStack itemStack)
    {
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

    private DataBaseItemStack GetAvailable(ItemMeta itemMeta)
    {
        foreach(DataBaseItemStack db in itemList)
        {
            if (db.itemMeta == itemMeta &&
                db.amount < db.itemMeta.GetMaxAmount())
                return db;
        }
        return null;
    }
}

[System.Serializable]
public class DataBaseItemStack
{
    public ItemMeta itemMeta;
    public int amount;
}
