using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpriteView : MonoBehaviour, IPunObservable
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private PhotonView photonView;

    private string currentSpriteName;

    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<SpriteRenderer>() != null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.Joined) return;

        if (stream.IsWriting)
        {
            //if (photonView.IsMine == false) return;
            Debug.LogError(photonView.ViewID + ": Is Writing");
            /*if (spriteRenderer.sprite == null) return;
            stream.SendNext(spriteRenderer.sprite.name);*/
            //Debug.LogError(photonView.ViewID + ": hello");
        }
        else if(stream.IsReading)
        {
            //if (photonView.IsMine == false) return;
            Debug.LogError(photonView.ViewID + ": Is Reading");
            /*if (stream.ReceiveNext() != null || (string)stream.ReceiveNext() != "")
            {
                currentSpriteName = (string)stream.ReceiveNext();
                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/" + currentSpriteName);
            }*/
        }
    }

    public void ChangeSprite(Sprite newSprite)
    {
            spriteRenderer.sprite = newSprite;
    }
}
