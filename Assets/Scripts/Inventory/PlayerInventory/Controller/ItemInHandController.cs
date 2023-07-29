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

    public Sprite test;


    private void Start()
    {
        
    }
    void Update()
    {
        if(PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Joined)
        {
            if (photonView.IsMine == false) return;
            if (PlayerInventoryController.itemInHand == null)
            {
                spriteView.ChangeSprite(null);
                return;
            }
            spriteView.ChangeSprite(PlayerInventoryController.itemInHand.GetSprite());
            test = PlayerInventoryController.itemInHand.GetSprite();
        }
        else
        {
            if (PlayerInventoryController.itemInHand == null)
            {
                spriteView.ChangeSprite(null);
                return;
            }
            spriteView.ChangeSprite(PlayerInventoryController.itemInHand.GetSprite());
        }
    }
}
