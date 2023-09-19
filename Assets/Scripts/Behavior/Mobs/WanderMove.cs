using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderMove : MonoBehaviour, IMovement
{
    [SerializeField]
    private float range = 5f;

    public float xInput { get; private set; }

    public float yInput { get; private set; }

    void Start()
    {
        xInput = Random.Range(-range, range);
        yInput = Random.Range(-range, range);
    }

    public void UpdateInput()
    {
        MakeRandomTarget();
    }

    public void MakeRandomTarget()
    {
        xInput = Random.Range(-range, range);
        yInput = Random.Range(-range, range);
    }

    public void Stop()
    {
        xInput = 0;
        yInput = 0;
    }
}
