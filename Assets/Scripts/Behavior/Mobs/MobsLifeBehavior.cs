using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobsLifeBehavior : MonoBehaviour, ILife
{
    [field: SerializeField]
    public string triggerDamageTag { get; private set; }

    public int Heal()
    {
        //throw new System.NotImplementedException();
        return 0;
    }

    // Start is called before the first frame update
    void Awake()
    {
        //triggerDamageTag = "Player";

        //triggerDamageTag = "PlayerAttack";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
