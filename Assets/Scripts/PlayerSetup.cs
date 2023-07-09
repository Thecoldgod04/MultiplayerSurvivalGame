using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerSetup : MonoBehaviourPun
{
    [SerializeField]
    private GameObject playerCamera;

    [SerializeField]
    private TextMeshProUGUI playerNameText;

    // Start is called before the first frame update
    void Start()
    {
        if(photonView.Owner == null)
            playerNameText.text = "TestClient";
        else
            playerNameText.text = photonView.Owner.NickName;

        if (photonView.ViewID != 0 && !photonView.IsMine) return;
        playerCamera.SetActive(true);
        playerCamera.tag = "MainCamera";
    }
}
