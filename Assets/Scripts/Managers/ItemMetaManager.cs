using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMetaManager : MonoBehaviour
{
    public static ItemMetaManager instance;

    private static int staticId;

    [field: SerializeField]
    public List<ItemMeta> itemMetaList { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        InitItemMetas();
    }

    private void InitItemMetas()
    {
        staticId = 0;
        foreach(ItemMeta i in itemMetaList)
        {
            i.Init(staticId);
            staticId++;
        }
    }

    public ItemMeta GetItemMetaById(int id)
    {
        foreach(ItemMeta i in itemMetaList)
        {
            if (i.GetId() == id)
                return i;
        }
        return null;
    }
}
