using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Drop List", menuName = "Drop List")]
[System.Serializable]
public class DropList : ScriptableObject
{
    [field: SerializeField]
    public List<DropItem> drops { get; private set; }
}

[System.Serializable]
public struct DropItem
{
    public ItemStack itemStack;
    public int maxAmount;
    [Range(0f, 1f)]
    public float possibility;
}
