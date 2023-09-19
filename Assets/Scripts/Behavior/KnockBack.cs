using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    [SerializeField]
    private float knockTime;

    float currentknockTime;

    [SerializeField]
    private float thrust = 2f;

    [SerializeField]
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        currentknockTime = knockTime;
    }

    Transform cause = null;
    private void FixedUpdate()
    {
        if (cause == null) return;
        Vector2 difference = transform.position - cause.transform.position;
        //Vector2 difference = stateMachine.GetPlayerTarget().transform.position - stateMachine.transform.position;
        difference = difference.normalized * thrust;
        rb.AddForce(difference, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        if(knocked)
        {
            if(currentknockTime <= 0)
            {
                rb.isKinematic = true;
                knocked = false;
            }
            else
            {
                //Debug.LogError(currentknockTime);
                currentknockTime -= Time.deltaTime;
            }
        }
    }

    bool knocked = false;
    public void Knock(Transform cause)
    {
        //Debug.LogError("Knocked");

        rb.isKinematic = false;

        knocked = true;

        currentknockTime = knockTime;

        this.cause = cause;
    }
}
