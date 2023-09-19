using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Crate : DataObject
{
    [SerializeField]
    private LootTable lootTable;

    [SerializeField]
    private CrateInventory crateInventory;

    [SerializeField]
    private bool isLootGenerated;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        crateInventory = GetComponent<CrateInventory>();

        //GenerateCrateLoot();
    }

    // Update is called once per frame
    protected override void Update()
    {
        /*if(photonView.IsMine && Input.GetKeyDown(KeyCode.Space))
        {
            photonView.RPC("DoShit", RpcTarget.AllBuffered);
        }*/
        //transform.position += (Vector3)(Vector2.left * 5f * Time.deltaTime);
    }

    public void GenerateCrateLoot()
    {
        if (isLootGenerated == true) return;
        isLootGenerated = true;

        Debug.LogError("Generating Loot");

        List<Vector2Int> lootData = LootTableManager.instance.GenerateLoot(lootTable);
        //Debug.LogError(lootData.Count);
        for (int i = 0; i < lootData.Count; i++)
        {
            Vector2Int tempLootData = lootData[i];
            //Debug.LogError(tempLootData.x);
            for (int j = 0; j < tempLootData.y; j++)
            {
                if (PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.Joined)
                {
                    //crateInventory.Add(lootTable.loots[tempLootData.x].itemMeta);
                    AddToCrateInventory(tempLootData.x);
                }
                else
                {
                    photonView.RPC("AddToCrateInventory", RpcTarget.AllBuffered, tempLootData.x);
                }
            }
        }
    }

    [PunRPC]
    public void AddToCrateInventory(int i)
    {
        if(crateInventory == null)
        {
            crateInventory = GetComponent<CrateInventory>();
        }

        crateInventory.Add(lootTable.loots[i].itemMeta);
        isLootGenerated = true;
    }
}
