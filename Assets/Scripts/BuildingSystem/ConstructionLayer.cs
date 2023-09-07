using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;
using Photon.Pun;

public class ConstructionLayer : TilemapLayer
{
    public Dictionary<Vector3Int, BuildableObject> occupiedList { get; private set; }

    [SerializeField]
    private GameObject dropItemTemplate;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        occupiedList = new Dictionary<Vector3Int, BuildableObject>();
    }

    public UnityEvent onConstructionBuild, onConstructionDestroy;

    public void Build(Vector3 coords, BuildableMeta buildableMeta)
    {
        if (IsEmpty(coords) == false) return;
        if (buildableMeta.GetBuildableType() == BuildableType.None) return;
        Vector3Int cellCoords = tilemap.WorldToCell(coords);

        GameObject gameObject = null;
        if(buildableMeta.GetBuildableType() == BuildableType.GameObject)
        {
            Vector3 worldCoord = tilemap.CellToWorld(cellCoords);
            worldCoord.x += 0.5f;
            worldCoord.y += 0.5f;
            worldCoord.z = -0.1f;
            //gameObject = Instantiate(buildableMeta.gameObject, worldCoord, Quaternion.identity);
            GameObject getFromPool = ObjectPool.instance.GetPooledObject(buildableMeta.gameObject);
            if(getFromPool != null)
            {
                gameObject = getFromPool;
                gameObject.transform.position = worldCoord;
                gameObject.SetActive(true);
            }
            else
            {
                gameObject = Instantiate(buildableMeta.gameObject, worldCoord, Quaternion.identity);
                Debug.LogWarning("Instantiated an object: " + buildableMeta.gameObject);
            }
        }

        TileChangeData tileChangeData = new TileChangeData(
            cellCoords,
            buildableMeta.tile,
            Color.white,
            Matrix4x4.Translate(new Vector3(0, 0.5f, 0))
            );
        //tilemap.SetTile(cellCoords, buildableMeta.tile);
        tilemap.SetTile(tileChangeData, false);

        BuildableObject buildableObject = new BuildableObject(buildableMeta, gameObject);

        occupiedList.Add(cellCoords, buildableObject);

        onConstructionBuild.Invoke();
    }
    public void Destroy(Vector3 position)
    { 
        Vector3Int cellCoord = tilemap.WorldToCell(position);

        if (IsEmpty(cellCoord)) return;
        if(occupiedList.ContainsKey(cellCoord))
        {
            if (occupiedList[cellCoord].realGameObject != null)
            {
                //Destroy(occupiedList[cellCoord].realGameObject);
                occupiedList[cellCoord].realGameObject.SetActive(false);
            }

            Vector3 pos = new Vector3(position.x, position.y, position.z + 1);
            SpawnConstructionDrop(occupiedList[cellCoord].buildableMeta, pos);
        }
        tilemap.SetTile(cellCoord, null);

        occupiedList.Remove(cellCoord);

        onConstructionDestroy.Invoke();
    }

    public void Load(Vector3 position)
    {
        Vector3Int cellCoord = tilemap.WorldToCell(position);
        if (IsEmpty(cellCoord)) return;

        if (occupiedList.ContainsKey(cellCoord))
        {
            if (occupiedList[cellCoord].realGameObject != null)
            {
                //Destroy(occupiedList[cellCoord].realGameObject);
                occupiedList[cellCoord].realGameObject.SetActive(true);
            }
        }
    }

    public void Unload(Vector3 position)
    {
        Vector3Int cellCoord = tilemap.WorldToCell(position);
        if (IsEmpty(cellCoord)) return;

        if (occupiedList.ContainsKey(cellCoord))
        {
            if (occupiedList[cellCoord].realGameObject != null)
            {
                //Destroy(occupiedList[cellCoord].realGameObject);
                occupiedList[cellCoord].realGameObject.SetActive(false);
            }
        }
    }

    public bool IsEmpty(Vector3 coords)
    {
        Vector3Int cellCoords = tilemap.WorldToCell(coords);
        return occupiedList.ContainsKey(cellCoords) == false && tilemap.GetTile(cellCoords) == null;
    }

    private void SpawnConstructionDrop(BuildableMeta buildableMeta, Vector3 pos)
    {
        if (dropItemTemplate.GetComponent<ItemStack>() == null) return;

        GameObject dropItem = null;

        if (PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.Joined)
        {
            dropItem = Instantiate(dropItemTemplate, pos, Quaternion.identity);
            dropItem.GetComponent<ItemStack>().SetItemMeta(buildableMeta);
        }
        else if (photonView.IsMine)
        {
            dropItem = PhotonNetwork.Instantiate(dropItemTemplate.name, pos, Quaternion.identity);
            dropItem.GetComponent<ItemStack>().photonView.RPC("SetItemMeta", RpcTarget.All, buildableMeta.GetId());
        }
    }

    public void PlaceTile(TileBase tile, Vector3 pos)
    {
        Vector3Int cellCoords = tilemap.WorldToCell(pos);
        tilemap.SetTile(cellCoords, tile);
    }
}
[System.Serializable]
public class BuildableObject
{
    public BuildableMeta buildableMeta { get; private set; }

    public GameObject realGameObject { get; private set; }

    public BuildableObject(BuildableMeta buildableMeta, GameObject gameObject = null)
    {
        this.buildableMeta = buildableMeta;
        this.realGameObject = gameObject;
    }
}
