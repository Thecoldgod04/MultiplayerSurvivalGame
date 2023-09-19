using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CrateManager : MonoBehaviourPun
{
    public static CrateManager instance;

    [SerializeField]
    private List<CrateData> crateDataList;

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
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

    [PunRPC]
    public void AddToCrateDataListRPC(byte[] crateDataStream)
    {
        CrateData crateData = (CrateData)CustomTypeSerializer.ByteArrayToObject(crateDataStream);
        crateDataList.Add(crateData);
    }

    public void AddToCrateDataList(CrateData crateData, bool isRPC)
    {
        if(!isRPC)
        {
            crateDataList.Add(crateData);
        }
        else
        {
            byte[] byteCrateData = CustomTypeSerializer.ObjectToByteArray(crateData);
            //photonView.RPC("", RpcTarget.AllBuffered, )
        }
    }
}

[System.Serializable]
public struct CrateData
{
    public Vector2 position;
    public CrateInventory crateInventory;
}
