using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DropManager : MonoBehaviourPun
{
    public static DropManager instance;

    [SerializeField]
    private GameObject dropItemTemplate;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SpawnDrop(LootTable dropList, Vector2 pos)
    {
        if (dropList == null) return;

        GameObject dropItem = null;

        float dropForce = 3f;
        Vector2 dropDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

        if (PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.Joined)
        {
            foreach (Loot drop in dropList.loots)
            {
                if (LootTableManager.instance.Possible(drop.possibility))
                {
                    dropItem = Instantiate(dropItemTemplate, pos, Quaternion.identity);
                    dropItem.GetComponent<ItemStack>().SetItemMeta(drop.itemMeta);
                    dropItem.GetComponent<Rigidbody2D>().AddForce(dropDirection * dropForce, ForceMode2D.Impulse);
                }
            }
        }
        else if (photonView.IsMine)
        {
            foreach (Loot drop in dropList.loots)
            {
                if (LootTableManager.instance.Possible(drop.possibility))
                {
                    //PhotonNetwork.Instantiate("Items/" + drop.itemStack.name, transform.position, Quaternion.identity);
                    dropItem = PhotonNetwork.Instantiate(dropItemTemplate.name, pos, Quaternion.identity);
                    dropItem.GetComponent<ItemStack>().photonView.RPC("SetItemMeta", RpcTarget.AllBuffered, drop.itemMeta.GetId());
                    dropItem.GetComponent<Rigidbody2D>().AddForce(dropDirection * dropForce, ForceMode2D.Impulse);
                }
            }
        }
        /*if(dropItem != null)
            dropItem.GetComponent<Rigidbody2D>().AddForce(dropDirection * dropForce, ForceMode2D.Impulse);*/
    }

    /*private bool CanDrop(DropItem drop)
    {
        float num = (int)(drop.possibility * 100);
        float random = Random.Range(0, 101);

        if (random <= num)
        {
            //Debug.LogError(random + " | " + num);
            return true;
        }

        return false;
    }*/
}
