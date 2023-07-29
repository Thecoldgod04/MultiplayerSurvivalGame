using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovement
{
    public float xInput { get; }
    public float yInput { get; }

    public void UpdateInput();

    public void Stop();

    //public void UpdateInput(Vector2 destination);
}
