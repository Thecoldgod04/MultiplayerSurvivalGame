using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerCustomize : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField playerNameTextField;

    // Start is called before the first frame update
    void Start()
    {
        playerNameTextField.text = PhotonNetwork.LocalPlayer.NickName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SubmitChanges()
    {
        PhotonNetwork.LocalPlayer.NickName = playerNameTextField.text;
    }
}
