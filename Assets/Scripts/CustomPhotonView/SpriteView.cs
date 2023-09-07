using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class SpriteView : MonoBehaviourPun, IPunObservable
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Image uiImage;

    [SerializeField]
    private string currentSpriteName;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<SpriteRenderer>() != null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        else if(GetComponent<Image>() != null)
        {
            uiImage = GetComponent<Image>();
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
            if(spriteRenderer == null && uiImage == null)
            {
                Logger.LogError("SpriteView", "Missing Sprite Renderer or UI Image!");
                return;
            }

            if ((spriteRenderer != null && spriteRenderer.sprite == null) || (uiImage != null && uiImage.sprite == null))
            {
                stream.SendNext("");
                //Debug.LogError(photonView.ViewID + ": tell the fake client to not display anything");
            }
            else
            {
                if(uiImage == null)
                    stream.SendNext(spriteRenderer.sprite.name);
                else
                    stream.SendNext(uiImage.sprite.name);
                //Debug.LogError(photonView.ViewID + ": tell the fake client to display the sprite: " + spriteRenderer.sprite.name);
            }
        }
        else if(stream.IsReading)
        {
            //if (photonView.IsMine == false) return;
            //Debug.LogError(photonView.ViewID + ": Is Reading");

            object receivedData = stream.ReceiveNext();
            //if ((string)receivedData == "") return;
            currentSpriteName = (string)receivedData;
            if (currentSpriteName == "")
            {
                if(spriteRenderer != null)
                    spriteRenderer.sprite = null;
                if(uiImage != null)
                    uiImage.sprite = null;
            }
            else
            {
                if(spriteRenderer != null)
                    spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/" + currentSpriteName);
                else if(uiImage != null)
                    uiImage.sprite = Resources.Load<Sprite>("Sprites/" + currentSpriteName);
            }

            //Debug.LogError(photonView.ViewID + ": The client over there tells me to display sprite named: " + currentSpriteName);
        }
    }

    public void ChangeSprite(Sprite newSprite)
    {
        if (spriteRenderer != null)
            spriteRenderer.sprite = newSprite;
        else if (uiImage != null)
            uiImage.sprite = newSprite;
    }
}
