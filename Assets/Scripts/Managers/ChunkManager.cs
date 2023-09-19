using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChunkManager : MonoBehaviour
{
    public static ChunkManager instance;

    [field: SerializeField]
    public int ChunkSize { get; private set; }

    public Dictionary<Vector2, GameObject> LoadedChunks { get; private set; }

    [field: SerializeField]
    // Storing the coordinates of the data objects that have already been loaded (could be by another player)
    public List<Vector3> LoadedDataObjects { get; private set; }    // x: x coord, y: y coord, z: DataObject's Id

    public Dictionary<Vector2, GameObject> ChunksOfLoadedDataObjects { get; private set; }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;

            LoadedChunks = new Dictionary<Vector2, GameObject>();

            LoadedDataObjects = new List<Vector3>();

            ChunksOfLoadedDataObjects = new Dictionary<Vector2, GameObject>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public bool IsInChunksOfLoadedDataObjects(Vector3 pos)
    {
        Vector2 pos2D = GetChunkPosition(pos);

        Vector2 loadedDataObjectChunkPos;
        foreach(Vector3 index in LoadedDataObjects)
        {
            loadedDataObjectChunkPos = new Vector2(index.x, index.y);
            if(loadedDataObjectChunkPos == pos2D)
            {
                return true;
            }
        }

        return false;
    }

    public int GetDataObjectIdByPosition(Vector2 pos)
    {
        int id = -1;

        Vector2 pos2D = new Vector2(pos.x, pos.y);

        Vector2 loadedDataObjectPos;
        foreach (Vector3 index in LoadedDataObjects)
        {
            loadedDataObjectPos = new Vector2(index.x, index.y);
            if (loadedDataObjectPos == pos2D)
            {
                id = (int)index.z;
            }
        }

        return id;
    }

    public Vector2 GetChunkPosition(Vector3 position)
    {
        // Round the position to the nearest multiple of chunkSize
        float x = Mathf.Floor(position.x / ChunkSize) * ChunkSize;
        float y = Mathf.Floor(position.y / ChunkSize) * ChunkSize;

        return new Vector2(x, y);
    }
}
