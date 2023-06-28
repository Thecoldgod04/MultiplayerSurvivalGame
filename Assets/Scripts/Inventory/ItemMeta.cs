using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Itemmeta", menuName = "Itemmeta")]
public class ItemMeta : ScriptableObject
{
    private static int staticId;

    [SerializeField]
    private int id;

    [SerializeField]
    private string itemName;

    public static int maxAmount = 12;

    [SerializeField]
    private Sprite icon;

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

    public Sprite GetIcon()
    {
        return icon;
    }

    public int GetMaxAmount()
    {
        return maxAmount;
    }
}
