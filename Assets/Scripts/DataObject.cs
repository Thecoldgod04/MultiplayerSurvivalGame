using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//  All classes extending this class are considered DataObject
//  DataObject: is a gameobject in the game but is spawned using PhotonNetwork.Instantiate()
//  => the DataObject will be instantiated in all clients so it needs to be handled properly
public class DataObject : MonoBehaviourPun
{
    [field: SerializeField]
    public Vector2 CurrentChunkPosition { get; private set; }

    public static int staticId = 0;

    [field: SerializeField]
    public int Id { get; private set; }

    [field: SerializeField]
    public bool NeedsActivation;

    [field: SerializeField]
    public bool Activated;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Id = staticId;

        staticId++;

        DataObjectManager.instance.DataObjectList.Add(this);

        if (!NeedsActivation)
            Activated = true;

        ObjectLoadedChunkCheck();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //ObjectLoadedChunkCheck();
    }

    private void ObjectLoadedChunkCheck()
    {
        if (!Activated) return;

        //if (transform.parent != null) return;

        if(!IsObjectInPlayerOwnerLoadedChunks())
        {
            //this.gameObject.SetActive(false);
            //Debug.LogError("Chest generated at chunk: " + CurrentChunkPosition);

            GameObject newChunkObject = null;
            if (transform.parent != null)
            {
                newChunkObject = new GameObject("" + CurrentChunkPosition);
                transform.SetParent(newChunkObject.transform);
                newChunkObject.SetActive(false);
                newChunkObject.transform.SetParent(ChunkManager.instance.transform);
            }
            else
            {
                newChunkObject = transform.parent.gameObject;
            }

            ChunkManager.instance.ChunksOfLoadedDataObjects.Add(CurrentChunkPosition, newChunkObject);
            //Debug.LogError(ChunkManager.instance.ChunksOfLoadedDataObjects.Count);
        }
        else
        {
            GameObject chunkObject = ChunkManager.instance.LoadedChunks[CurrentChunkPosition];

            this.transform.SetParent(chunkObject.transform);

            this.gameObject.SetActive(true);
        }
    }

    private bool IsObjectInPlayerOwnerLoadedChunks()
    {
        PlayerChunkTracer playerChunkTracer = GameManager.instance.PlayerOwner.GetComponentInChildren<PlayerChunkTracer>();

        Debug.LogWarning(playerChunkTracer == null);

        CurrentChunkPosition = ChunkManager.instance.GetChunkPosition(transform.position);
        List<Vector2> chunksAroundPlayer = playerChunkTracer.GetChunksAroundPlayer();

        return chunksAroundPlayer.Contains(CurrentChunkPosition);
        //return true;
    }
}
