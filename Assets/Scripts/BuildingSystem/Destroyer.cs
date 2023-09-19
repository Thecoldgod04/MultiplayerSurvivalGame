using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Destroyer : MonoBehaviourPun
{
    [SerializeField]
    private ConstructionLayer constructionLayer;

    private float destroyRange;

    // Start is called before the first frame update
    void Start()
    {
        destroyRange = Builder.buildRange;

        if(ConstructionLayer.instance == null)
        {
            constructionLayer = FindObjectOfType<ConstructionLayer>();
        }
        else
        {
            constructionLayer = ConstructionLayer.instance;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.Joined ||
           photonView.IsMine == true)
        {
            if (UIManager.instance.IsUsingUI()) return;
            DestroyInputCheck();
        }
    }

    private void DestroyInputCheck()
    {
        if (Camera.main == null) return;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (IsInRange(mousePos) == false) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.Joined)
                RequestDestroy(mousePos);
            else
                photonView.RPC("RequestDestroy", RpcTarget.AllBuffered, mousePos);
        }
    }

    [PunRPC]
    public void RequestDestroy(Vector3 pos)
    {
        if (ConstructionLayer.instance == null)
        {
            constructionLayer = FindObjectOfType<ConstructionLayer>();
        }
        else
        {
            constructionLayer = ConstructionLayer.instance;
        }

        constructionLayer.Destroy(pos);
    }

    public bool IsInRange(Vector3 mousePos)
    {
        //Debug.LogError(Vector3.Distance(transform.position, mousePos));
        return Vector2.Distance(transform.position, mousePos) <= destroyRange;
    }
}
