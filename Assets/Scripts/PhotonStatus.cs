using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PhotonStatus : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI photonStatusText;

    // Start is called before the first frame update
    void Start()
    {
        photonStatusText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        photonStatusText.text = PhotonNetwork.NetworkClientState.ToString();
    }
}
