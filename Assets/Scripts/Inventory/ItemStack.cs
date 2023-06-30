using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[System.Serializable]
public class ItemStack : MonoBehaviourPun
{
    [SerializeField]
    private ItemMeta itemMeta;

    [SerializeField]
    [Range(1, 12)]
    private int amount;

    [SerializeField]
    private SpriteRenderer spriteRenderer;


    [SerializeField]
    private PlayerInventoryController inventoryController;

    private void Start()
    {
        spriteRenderer.sprite = itemMeta.GetIcon();
    }

    public void SetItemMeta(ItemMeta itemMeta)
    {
        this.itemMeta = itemMeta;
    }

    public ItemMeta GetItemMeta()
    {
        return itemMeta;
    }

    public void SetAmount(int amount)
    {
        this.amount = amount;
    }

    public void AddAmount(int amount)
    {
        this.amount += amount;
    }

    public int GetAmount()
    {
        return amount;
    }

    //Event firing
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Joined)
            {
                if (collision.GetComponent<PhotonView>().IsMine == true)
                {
                    inventoryController.OnItemCollected(this);
                }
            }
            else
            {
                inventoryController.OnItemCollected(this);
            }
            Destroy(this.gameObject);
        }
    }
}
