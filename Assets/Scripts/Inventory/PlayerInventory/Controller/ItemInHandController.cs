using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ItemInHandController : MonoBehaviour
{
    //public static ItemInHandController instance;

    [SerializeField]
    private SpriteView spriteView;

    [SerializeField]
    private PhotonView photonView;


    private void Start()
    {
        
    }
    void Update()
    {
        if(PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Joined)
        {
            if (photonView.IsMine == false) return;
            spriteView.ChangeSprite(PlayerInventoryController.itemInHandSprite);
        }
        else
        {
            spriteView.ChangeSprite(PlayerInventoryController.itemInHandSprite);
        }
    }
}
