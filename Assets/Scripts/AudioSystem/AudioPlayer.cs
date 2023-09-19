using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AudioPlayer : MonoBehaviourPun
{
    [SerializeField]
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAudio()
    {
        if(PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.Joined)
        {
            PlayAudioRPC();
        }
        else
        {
            photonView.RPC("PlayAudioRPC", RpcTarget.All);
        }
    }

    [PunRPC]
    private void PlayAudioRPC()
    {
        audioSource.Play();
    }
}
