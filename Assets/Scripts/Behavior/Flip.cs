using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flip : MonoBehaviour
{
    [SerializeField]
    private Transform movingObject;

    private bool facingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        movingObject = transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoFlipByInput(float input)
    {
        if (input > 0 && facingRight == false)
        {
            Flipping();
        }
        else if (input < 0 && facingRight == true)
        {
            Flipping();
        }
        else
        {
            return;
        }
    }

    private void Flipping()
    {
        movingObject.transform.localScale = new Vector3(-movingObject.transform.localScale.x, movingObject.transform.localScale.y, movingObject.transform.localScale.z);
        facingRight = !facingRight;
    }
}
