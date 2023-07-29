using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Droppable : MonoBehaviourPun
{
    [SerializeField]
    private DropList dropList;

    private void Start()
    {
        //SpawnDrop();
    }

    public void SpawnDrop()
    {
        if (PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.Joined)
        {
            foreach(DropItem drop in dropList.drops)
            {
                if(CanDrop(drop))
                {
                    ItemStack item = Instantiate(drop.itemStack, transform.position, Quaternion.identity);
                }
            }
        }
        else if(PhotonNetwork.IsMasterClient)
        {
            foreach (DropItem drop in dropList.drops)
            {
                if (CanDrop(drop))
                {
                    PhotonNetwork.Instantiate("Items/" + drop.itemStack.name, transform.position, Quaternion.identity);
                }
            }
        }
    }

    private bool CanDrop(DropItem drop)
    {
        float num = (int) (drop.possibility * 100);
        float random = Random.Range(0, 101);

        if(random <= num)
        {
            //Debug.LogError(random + " | " + num);
            return true;
        }

        return false;
    }
}
