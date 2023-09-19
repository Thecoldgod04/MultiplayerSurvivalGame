using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.PlayerOwner.transform;
        //transform.SetParent(player.parent);
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;
        transform.position = player.position;

        /*if(player == null)
        {
            transform.SetParent(null);
        }*/
    }
}
