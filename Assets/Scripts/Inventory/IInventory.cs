using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public interface IInventory
{
    public void SetInventory(List<DataBaseItemStack> itemList);
    public int Add(ItemStack itemStack);

    public int Add(ItemMeta itemMeta);

    public int AddRPC(int itemMetaId);

    public int Remove(ItemMeta itemMeta);

    public int RemoveRPC(int itemMetaId);

    public PhotonView GetPhotonView();

    public bool Contains(ItemMeta itemMeta);

    public int GetIndex(DataBaseItemStack db);

    public DataBaseItemStack Get(int index);

    public List<DataBaseItemStack> GetItemList();

    public DataBaseItemStack GetAvailable(ItemMeta itemMeta);

    public bool IsFull();
}
