using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTargetMove : MonoBehaviour, IMovement
{
    [SerializeField]
    protected Transform target;

    public float xInput { get; private set; }

    public float yInput { get; private set; }

    public void UpdateInput()
    {
        xInput = target.position.x - transform.position.x;
        yInput = target.position.y - transform.position.y;
    }
    public virtual void Stop()
    {
        xInput = 0;
        yInput = 0;
        target = null;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
