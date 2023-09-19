using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public enum MobType
{
    Hostile,
    Passive
}

public class Mob : MonoBehaviourPun
{
    [SerializeField]
    private int Id;

    [SerializeField]
    private MobType mobType;

    

    // Start is called before the first frame update
    void Start()
    {
        if(PhotonNetwork.NetworkClientState != ClientState.Joined)
        {
            MobManager.instance.MobList.Add(this);
        }
        else
        {
            MobManager.instance.photonView.RPC("AddMobToListRPC", RpcTarget.AllBuffered, photonView.ViewID);
        }
    }

    bool despawn = false;
    // Update is called once per frame
    void Update()
    {
        if(PhotonNetwork.NetworkClientState != ClientState.Joined)
        {
            Transform player = GameManager.instance.PlayerOwner.transform;
            if (Vector2.Distance(transform.position, player.transform.position) >= MobManager.instance.DespawnDistance)
            {
                despawn = true;
            }
            else
            {
                despawn = false;
            }
        }
        else
        {
            foreach (GameObject player in GameManager.instance.PlayerList)
            {
                if (Vector2.Distance(transform.position, player.transform.position) >= MobManager.instance.DespawnDistance)
                {
                    despawn = true;
                }
                else
                {
                    despawn = false;
                }
            }
        }

        if(despawn == true && mobType != MobType.Passive)
        {
            Despawn();
        }
    }

    private void Despawn()
    {
        this.gameObject.SetActive(false);

        if (PhotonNetwork.NetworkClientState != ClientState.Joined)
        {
            MobManager.instance.MobList.Remove(this);
        }
        else
        {
            MobManager.instance.photonView.RPC("RemoveFromMobListRPC", RpcTarget.AllBuffered, photonView.ViewID);
        }
    }
}
