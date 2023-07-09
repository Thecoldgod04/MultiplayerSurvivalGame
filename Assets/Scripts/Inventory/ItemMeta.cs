using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemMeta", menuName = "ItemMeta")]
public class ItemMeta : ScriptableObject
{
    [SerializeField]
    private int id;

    [SerializeField]
    private string itemName;

    public static int maxAmount = 12;

    [SerializeField]
    private Sprite sprite;

    public virtual void Init(int id)
    {
        this.id = id;
    }

    public void SetName(string name)
    {
        this.name = name;
    }

    public string GetName()
    {
        return name;
    }

    public int GetId()
    {
        return id;
    }

    public Sprite GetSprite()
    {
        return sprite;
    }

    public int GetMaxAmount()
    {
        return maxAmount;
    }
}
