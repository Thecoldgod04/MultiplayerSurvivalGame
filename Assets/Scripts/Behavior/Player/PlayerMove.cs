using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMove : MonoBehaviourPun, IMovement
{
    public float xInput { get; private set; }

    public float yInput { get; private set; }

    [SerializeField]
    private Flip flipBehavior;

    // Start is called before the first frame update
    void Start()
    {
        if(GetComponentInChildren<Flip>() != null)
            flipBehavior = GetComponentInChildren<Flip>();   
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateInput();
    }

    public void UpdateInput()
    {
        if (photonView.ViewID != 0 && !photonView.IsMine || UIManager.instance.IsUsingUI()) return;

        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        //flipBehavior.DoFlipByInput(xInput);
    }

    public void Stop()
    {
        xInput = 0;
        yInput = 0;
    }
}
