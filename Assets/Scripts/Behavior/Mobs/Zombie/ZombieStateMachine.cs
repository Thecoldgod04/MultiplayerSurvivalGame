using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieStateMachine : MonoBehaviour
{
    [SerializeField]
    private IZomebieState currentState;

    #region States
    [field: SerializeField]
    public ZombieNaiveState naiveState { get; private set; }

    [field: SerializeField]
    public ZombieIdleState idleState { get; private set; }

    [field: SerializeField]
    public ZombieChaseState chaseState { get; private set; }

    [field: SerializeField]
    public ZombieWanderState wanderState { get; private set; }
    #endregion

    #region Behaviors
    public Movement movementBehavior { get; private set; }
    public WanderMove wanderMoveInput { get; private set; }
    public FollowTargetMove followTargetMoveInput { get; private set; }
    public Flip flipBehavior { get; private set; }
    #endregion

    [field: SerializeField]
    public float detectRange { get; private set; }

    [field: SerializeField]
    public float chaseRange { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        naiveState = new ZombieNaiveState();
        idleState = new ZombieIdleState();
        chaseState = new ZombieChaseState();
        wanderState = new ZombieWanderState();

        movementBehavior = GetComponent<Movement>();
        /*flipBehavior = GetComponentInChildren<Flip>();*/
        wanderMoveInput = GetComponent<WanderMove>();
        followTargetMoveInput = GetComponent<FollowTargetMove>();

        currentState = idleState;
    }

    // Update is called once per frame
    void Update()
    {
        currentState.DoStateUpdate(this);
        Debug.LogError(currentState);
    }

    private void FixedUpdate()
    {
        currentState.DoStateFixedUpdate(this);
    }

    public void SetState(IZomebieState state)
    {
        currentState = state;
    }


    void OnDrawGizmos()
    {
        detectRange = 4f;
        chaseRange = 6f;

        Gizmos.color = Color.red; 
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
