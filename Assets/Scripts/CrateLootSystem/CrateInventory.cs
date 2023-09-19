using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CrateInventory : MonoBehaviourPun, IInventory
{
    [field: SerializeField]
    public int InventorySize { get; private set; }

    [field: SerializeField]
    public List<DataBaseItemStack> ItemList { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        //ItemList = new List<DataBaseItemStack>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetInventory(List<DataBaseItemStack> itemList)
    {
        this.ItemList = itemList;
    }

    public int Add(ItemStack itemStack)
    {
        int index = -1;
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
            ItemList.Add(newDb);
            index = GetIndex(newDb);
            //Debug.LogError(ItemList.Count);
        }
        return index;
    }

    [PunRPC]
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

            if (available.amount == 1)
            {
                ItemList.Remove(available);
            }
            else
            {
                GetAvailable(itemMeta).amount--;
            }
        }

        return index;
    }

    [PunRPC]
    public int RemoveRPC(int itemMetaId)
    {
        ItemMeta itemMeta = ItemMetaManager.instance.GetItemMetaById(itemMetaId);
        return Remove(itemMeta);
    }

    public PhotonView GetPhotonView()
    {
        return photonView;
    }

    public bool Contains(ItemMeta itemMeta)
    {
        foreach (DataBaseItemStack db in ItemList)
        {
            if (db.itemMeta == itemMeta)
                return true;
        }
        return false;
    }

    public int GetIndex(DataBaseItemStack db)
    {
        return ItemList.IndexOf(db);
    }

    public DataBaseItemStack Get(int index)
    {
        return ItemList[index];
    }

    public List<DataBaseItemStack> GetItemList()
    {
        return ItemList;
    }

    public DataBaseItemStack GetAvailable(ItemMeta itemMeta)
    {
        foreach (DataBaseItemStack db in ItemList)
        {
            if (db.itemMeta == itemMeta &&
                db.amount < db.itemMeta.GetMaxAmount())
                return db;
        }
        return null;
    }

    public bool IsFull()
    {
        return ItemList.Count >= InventorySize;
    }
}
