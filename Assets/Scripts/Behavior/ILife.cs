using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILife
{
    public string triggerDamageTag { get; }

    public int Heal();
}
