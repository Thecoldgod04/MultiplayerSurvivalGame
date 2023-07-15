using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerLifeBehavior : MonoBehaviour, ILife
{
    [SerializeField]
    private int healAmount;

    [SerializeField]
    private KeyCode healKey;

    [field: SerializeField]
    public string triggerDamageTag { get; private set; }

    public int Heal()
    {
        if(Input.GetKeyDown(healKey))
        {
            return healAmount;
        }
        return 0;
    }

    // Start is called before the first frame update
    void Awake()
    {
        triggerDamageTag = "Enemy";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
