using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;
using Photon.Pun;

public class ConstructionLayer : TilemapLayer
{
    public static ConstructionLayer instance;

    public Dictionary<Vector3Int, BuildableObject> occupiedList { get; private set; }

    [SerializeField]
    private GameObject dropItemTemplate;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        occupiedList = new Dictionary<Vector3Int, BuildableObject>();

        SaveLoadManager.instance.LoadGameRPC();
    }

    public UnityEvent onConstructionBuild, onConstructionDestroy;

    //[PunRPC]
    public void RegisterObjectToOccupiedList(Vector3 pos, BuildableObject buildableObject)
    {
        Vector3Int cellCoords = tilemap.WorldToCell(pos);

        if (occupiedList.ContainsKey(cellCoords)) return;

        occupiedList.Add(cellCoords, buildableObject);
    }

    public GameObject Build(Vector3 coords, BuildableMeta buildableMeta)
    {
        if (IsEmpty(coords) == false) return null;
        if (buildableMeta.GetBuildableType() == BuildableType.None) return null;
        Vector3Int cellCoords = tilemap.WorldToCell(coords);

        GameObject gameObject = null;
        if(buildableMeta.GetBuildableType() == BuildableType.GameObject || buildableMeta.GetBuildableType() == BuildableType.DataGameObject)
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
                if(PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Joined)
                {
                    if (buildableMeta.GetBuildableType() != BuildableType.DataGameObject)
                    {
                        gameObject = Instantiate(buildableMeta.gameObject, worldCoord, Quaternion.identity);
                        if (gameObject == null)
                        {
                            Debug.LogError("Null gameobject with meta_name: " + buildableMeta.name);
                        }
                    }
                    else
                    {
                        //Debug.LogError("Spawning network bug :)))");
                        //gameObject = PhotonNetwork.Instantiate(buildableMeta.gameObject.name, worldCoord, Quaternion.identity);
                        if(PhotonNetwork.IsMasterClient)
                        {
                            RequestMasterToBuild(buildableMeta.gameObject.name, worldCoord);
                        }
                        else
                        {
                            photonView.RPC("RequestMasterToBuild", RpcTarget.MasterClient, buildableMeta.gameObject.name, worldCoord);
                        }
                    }
                }
                else
                {
                    gameObject = Instantiate(buildableMeta.gameObject, worldCoord, Quaternion.identity);
                    if (gameObject == null)
                    {
                        Debug.LogError("Null gameobject with meta_name: " + buildableMeta.name);
                    }
                }

                //gameObject = Instantiate(buildableMeta.gameObject, worldCoord, Quaternion.identity);

                //Debug.LogWarning("Instantiated an object: " + buildableMeta.gameObject);
                if(gameObject != null)
                    ObjectPool.instance.AddToPool(gameObject);
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

        //occupiedList.Add(cellCoords, buildableObject);

        RegisterObjectToOccupiedList(cellCoords, buildableObject);

        onConstructionBuild.Invoke();

        return gameObject;
    }

    [PunRPC]
    public GameObject RequestMasterToBuild(string objectName, Vector3 position)
    {
        GameObject go = null;
        if(PhotonNetwork.IsMasterClient)
        {
            go = PhotonNetwork.Instantiate(objectName, position, Quaternion.identity);
        }
        return go;
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

    public GameObject Load(Vector3 position)
    {
        Vector3Int cellCoord = tilemap.WorldToCell(position);
        if (IsEmpty(cellCoord)) return null;

        GameObject gameObject = null;

        if (occupiedList.ContainsKey(cellCoord))
        {
            if (occupiedList[cellCoord].realGameObject != null)
            {
                Vector3 worldCoord = tilemap.CellToWorld(cellCoord);
                worldCoord.x += 0.5f;
                worldCoord.y += 0.5f;
                worldCoord.z = -0.1f;

                GameObject getFromPool = ObjectPool.instance.GetPooledObject(occupiedList[cellCoord].realGameObject);

                if (getFromPool != null)
                {
                    gameObject = getFromPool;
                    gameObject.transform.position = worldCoord;
                    gameObject.SetActive(true);
                }
                else
                {
                    //occupiedList[cellCoord].realGameObject.SetActive(true);
                    //gameObject = occupiedList[cellCoord].realGameObject;
                }
            }
        }

        return gameObject;
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

        /*GameObject dropItem = null;

        if (PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.Joined)
        {
            dropItem = Instantiate(dropItemTemplate, pos, Quaternion.identity);
            dropItem.GetComponent<ItemStack>().SetItemMeta(buildableMeta);
        }
        else if (photonView.IsMine)
        {
            dropItem = PhotonNetwork.Instantiate(dropItemTemplate.name, pos, Quaternion.identity);
            dropItem.GetComponent<ItemStack>().photonView.RPC("SetItemMeta", RpcTarget.AllBuffered, buildableMeta.GetId());
        }*/
        DropManager.instance.SpawnDrop(buildableMeta.lootTable, pos);
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
