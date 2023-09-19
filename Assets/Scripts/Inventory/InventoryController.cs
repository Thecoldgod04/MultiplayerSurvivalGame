using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class InventoryController : MonoBehaviourPun
{
    protected IInventory inventory;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void AddToInventory(ItemMeta itemMeta)
    {
        //inventory.Add(itemMeta);

        int itemMetaId = ItemMetaManager.instance.itemMetaList.IndexOf(itemMeta);

        if(itemMetaId >= 0)
        {
            if (PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.Joined || photonView == null)
            {
                inventory.AddRPC(itemMetaId);
            }
            else if (inventory.GetPhotonView() != null)
            {
                inventory.GetPhotonView().RPC("AddRPC", RpcTarget.AllBuffered, itemMetaId);
            }
        }

        if (PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.Joined || photonView == null)
        {
            UpdateUI();
        }
        else if (inventory.GetPhotonView() != null)
        {
            photonView.RPC("UpdateUI", RpcTarget.AllBuffered);
        }
    }

    public virtual void RemoveFromInventory(ItemMeta itemMeta)
    {
        int itemMetaId = ItemMetaManager.instance.itemMetaList.IndexOf(itemMeta);

        if (itemMetaId >= 0)
        {
            if (PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.Joined || photonView == null)
            {
                //Debug.LogError("Remove Standard");
                inventory.RemoveRPC(itemMetaId);
            }
            else if (inventory.GetPhotonView() != null)
            {
                //Debug.LogError("Remove RPC");
                inventory.GetPhotonView().RPC("RemoveRPC", RpcTarget.AllBuffered, itemMetaId);
            }
        }

        if (PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.Joined || photonView == null)
        {
            UpdateUI();
        }
        else if (inventory.GetPhotonView() != null)
        {
            //Debug.LogError(photonView.ViewID);
            photonView.RPC("UpdateUI", RpcTarget.AllBuffered);
        }
    }

    public IInventory GetInventory()
    {
        return inventory;
    }

    public void UpdateUiRPC()
    {
        UpdateUI();
    }

    public abstract void UpdateUI();
}
