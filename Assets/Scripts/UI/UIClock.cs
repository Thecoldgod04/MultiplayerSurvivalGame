using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIClock : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI clockUiText;

    // Update is called once per frame
    void Update()
    {
        clockUiText.text = WorldTime.instance.GetCurrentTime().ToString();
    }
}
