using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataObjectManager : MonoBehaviour
{
    public static DataObjectManager instance;

    [field: SerializeField]
    public List<DataObject> DataObjectList { get; private set; }
    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;

            DataObjectList = new List<DataObject>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public DataObject GetAt(int index)
    {
        return DataObjectList[index];
    }
}
