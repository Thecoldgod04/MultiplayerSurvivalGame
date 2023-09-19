using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Movement : MonoBehaviourPunCallbacks
{
    public IMovement moveInput { get; private set; }

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<IMovement>() != null) moveInput = GetComponent<IMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (!photonView.IsMine) return;
        //CalculateVelocity();
    }

    private void FixedUpdate()
    {
        //Move();
    }

    public void SetMoveInput(IMovement moveInput)
    {
        this.moveInput = moveInput;
    }

    Vector2 velocity;
    public void CalculateVelocity()
    {
        velocity = new Vector2(moveInput.xInput, moveInput.yInput);

        //Debug.LogError(moveInput.xInput + " : " + moveInput.yInput);

        velocity.Normalize();
    }

    public void ApplyVelocity()
    {
        rb.velocity = (velocity * moveSpeed) * Time.fixedDeltaTime;
    }
}
