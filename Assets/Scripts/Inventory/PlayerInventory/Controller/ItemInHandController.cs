using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ItemInHandController : MonoBehaviour
{
    /*[SerializeField]
    private SpriteRenderer itemInHandRenderer;*/

    [SerializeField]
    private SpriteView spriteView;

    [SerializeField]
    private PhotonView photonView;

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine == false)
        {
            //Debug.LogError(transform.name + ": photonView.IsMine == false");
            return;
        }
        spriteView.ChangeSprite(PlayerInventoryController.itemInHandSprite);
    }
}
