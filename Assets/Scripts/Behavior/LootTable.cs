using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Loot Table", menuName = "Loot Table")]
[System.Serializable]
public class LootTable : ScriptableObject
{
    [field: SerializeField]
    public List<Loot> loots { get; private set; }
}

[System.Serializable]
public struct Loot
{
    //public ItemStack itemStack;
    public ItemMeta itemMeta;
    public int maxAmount;
    [Range(0f, 1f)]
    public float possibility;
}
