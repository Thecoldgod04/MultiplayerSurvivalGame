using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerChunkTracer : MonoBehaviourPun
{
    //[field: SerializeField]
    private int chunkSize;

    [SerializeField]
    private Transform player;

    private Vector2 previousChunkPosition;

    [SerializeField]
    private BiomeLayer biomeLayer;

    [SerializeField]
    private ConstructionLayer constructionLayer;

    [SerializeField]
    private EnvironmentGenerator environmentGenerator;

    [SerializeField]
    private MapGenerator mapGenerator;

    private Dictionary<Vector2, GameObject> loadedChunks;

    private Dictionary<Vector2, GameObject> chunksOfLoadedDataObjects;

    [SerializeField]
    private List<Vector2> chunksAroundPlayer = new List<Vector2>();

    [SerializeField]
    private List<Vector2> activeChunks = new List<Vector2>();

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Joined && !photonView.IsMine) return;

        biomeLayer = FindObjectOfType<BiomeLayer>();
        environmentGenerator = FindObjectOfType<EnvironmentGenerator>();
        mapGenerator = FindObjectOfType<MapGenerator>();
        constructionLayer = FindObjectOfType<ConstructionLayer>();

        chunkSize = ChunkManager.instance.ChunkSize;
        loadedChunks = ChunkManager.instance.LoadedChunks;
        chunksOfLoadedDataObjects = ChunkManager.instance.ChunksOfLoadedDataObjects;

        previousChunkPosition = GetChunkPosition(player.position);
        LoadChunks(previousChunkPosition);

        UnloadChunks(Vector2.zero, Vector2.zero);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player has moved to a different chunk
        if (PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Joined && !photonView.IsMine) return;

        Vector2 currentChunkPosition = GetChunkPosition(player.position);
        if (currentChunkPosition != previousChunkPosition)  // The player has moved to a different chunk
        {
            // Update the previous chunk position
            previousChunkPosition = currentChunkPosition;
            //Debug.LogError("Player moved to a different chunk: " + currentChunkPosition);

            // Load the nearer chunks
            LoadChunks(currentChunkPosition);
            // Unload further chunks
            UnloadChunks(previousChunkPosition, currentChunkPosition);
        }
    }

    private Vector2 GetChunkPosition(Vector3 position)
    {
        // Round the position to the nearest multiple of chunkSize
        /*float x = Mathf.Floor(position.x / chunkSize) * chunkSize;
        float y = Mathf.Floor(position.y / chunkSize) * chunkSize;

        return new Vector2(x, y);*/
        return ChunkManager.instance.GetChunkPosition(position);
    }

    bool isGameStarted = true;
    private void LoadChunks(Vector2 currentChunkPosition)
    {
        chunksAroundPlayer.Clear();
        for(int xOffset = -1; xOffset <= 1; xOffset++)
        {
            for(int yOffset = -1; yOffset <= 1; yOffset++)
            {
                Vector2Int startPos = new Vector2Int((int)(currentChunkPosition.x + xOffset * chunkSize), (int)(currentChunkPosition.y + yOffset * chunkSize));
                Vector2Int endPos = new Vector2Int(startPos.x + chunkSize, startPos.y + chunkSize);

                //Vector2 pos2D = new Vector2(pos.x, pos.y);
                /*if (loadedChunks.ContainsKey(startPos))
                {
                    loadedChunks[startPos].SetActive(true);
                    Debug.LogWarning("Loading loaded chunks");
                }
                else
                {
                    
                    Debug.LogWarning("Loading new chunks");
                }*/

                if (startPos.x >= 0 && startPos.y >= 0)
                {
                    chunksAroundPlayer.Add(startPos);
                }
                
                LoadOneChunk(startPos, endPos);

                if(!activeChunks.Contains(startPos))
                {
                    activeChunks.Add(startPos);
                }
            }
        }

        if(isGameStarted)
        {
            isGameStarted = false;
        }


        /*int startX = (int) currentChunkPosition.x - chunkSize;
        int startY = (int) currentChunkPosition.y - chunkSize;

        int endX = (int)currentChunkPosition.x + chunkSize*2;
        int endY = (int)currentChunkPosition.y + chunkSize*2;

        for(int y = startY; y <= endY; y++)
        {
            for(int x = startX; x <= endX; x++)
            {
                Vector3 pos = new Vector3(x, y);
                //Loading biomes
                biomeLayer.GenerateBiome(pos);

                //Loading environment
                if(loadedChunks.Contains(currentChunkPosition))
                {
                    environmentGenerator.LoadEnvironment(pos);
                }
                else
                {
                    environmentGenerator.GenerateEnvironment(pos);
                }
            }
        }
        loadedChunks.Add(currentChunkPosition);*/
    }   

    private void LoadOneChunk(Vector2Int posStart, Vector2Int posEnd)
    {
        bool containsDataObjects = false;

        if ((loadedChunks.ContainsKey(posStart) && loadedChunks[posStart].activeInHierarchy && !isGameStarted) || 
            (chunksOfLoadedDataObjects.ContainsKey(posStart) && chunksOfLoadedDataObjects[posStart].activeInHierarchy && !isGameStarted))
        {
            //Debug.LogWarning("Active chunk detected at: " + posStart);
            return;
        }


        if (posStart.x < 0 || posStart.y < 0) return;

        //Debug.LogError(posStart);

        //if ((posStart.x != 10 || posStart.y != 0) && (posStart.x != 0 || posStart.y != 0)) return;    //this line is only for debugging

        GameObject chunkGameObject = null;

        if(!loadedChunks.ContainsKey(posStart))
        {
            //loadedChunks.Add(posStart, chunkGameObject);
            if(chunksOfLoadedDataObjects.ContainsKey(posStart))
            {
                chunkGameObject = chunksOfLoadedDataObjects[posStart];
                chunkGameObject.SetActive(true);
                containsDataObjects = true;
            }
            else
            {
                chunkGameObject = new GameObject("" + posStart);
                chunkGameObject.transform.SetParent(ChunkManager.instance.transform);
            }
        }
        else
        {
            chunkGameObject = loadedChunks[posStart];
            loadedChunks[posStart].SetActive(true);
        }

        //posEnd.x -= 1;
        //posEnd.y -= 1;

        for(int y = posStart.y; y < posEnd.y; y++)
        {
            for(int x = posStart.x; x < posEnd.x; x++)
            {
                Vector3 pos = new Vector3(x, y);
                biomeLayer.GenerateBiome(pos);

                GameObject environmentObj = null;
                if (loadedChunks.ContainsKey(posStart))
                {
                    //Debug.LogWarning("Loading chunk: " + posStart);
                    environmentObj = environmentGenerator.LoadEnvironment(pos);
                }
                else
                {
                    Debug.LogWarning("Generating chunk: " + posStart + " from " + posStart + " to " + posEnd);
                    environmentObj = environmentGenerator.GenerateEnvironment(pos, containsDataObjects);
                }

                if (environmentObj != null && chunkGameObject != null)
                {
                    environmentObj.transform.SetParent(chunkGameObject.transform);
                }
            }
        }

        if (!loadedChunks.ContainsKey(posStart))
        {
            loadedChunks.Add(posStart, chunkGameObject);
            chunksOfLoadedDataObjects.Remove(posStart);
        }
    }
    
    private void UnloadChunks(Vector2 previousChunkPosition, Vector2 currentChunkPosition)
    {
        /*List<Vector2> checkedPos = new List<Vector2>();
        foreach (Vector2 chunkPos in activeChunks)
        {
            if (!chunksAroundPlayer.Contains(chunkPos))
            {
                loadedChunks[Vector2Int.RoundToInt(chunkPos)].SetActive(false);
                checkedPos.Add(chunkPos);
            }
        }
        foreach (Vector2 checks in checkedPos)
        {
            activeChunks.Remove(checks);
        }
        checkedPos.Clear();*/

        foreach (KeyValuePair<Vector2, GameObject> p in loadedChunks)
        {
            if (!chunksAroundPlayer.Contains(p.Key))
            {
                p.Value.SetActive(false);
                foreach(Transform obj in p.Value.transform.GetComponentsInChildren<Transform>())
                {
                    if(ObjectPool.instance.IsInPool(obj.gameObject))
                    {
                        obj.position = Vector2.zero;
                        obj.gameObject.SetActive(false);
                        obj.SetParent(null);
                    }
                }
            }
        }

        //activeChunks.
        /*int startX = 0;
        int startY = 0;

        int endX = 0;
        int endY = 0;
        if(previousChunkPosition.x - currentChunkPosition.x < 0)    //Go right
        {
            startX = (int)previousChunkPosition.x - chunkSize * 2;
            startY = (int)previousChunkPosition.y - chunkSize * 2;

            endX = startX + chunkSize * 2;
            endY = startY + chunkSize * (1 + 3 + 1);
        }
        else if(previousChunkPosition.x - currentChunkPosition.x > 0)   //Go left
        {
            startX = (int)previousChunkPosition.x + chunkSize;
            startY = (int)previousChunkPosition.y - chunkSize * 2;

            endX = startX + chunkSize * 2;
            endY = startY + chunkSize * (1 + 3 + 1);
        }
        else if (previousChunkPosition.y - currentChunkPosition.y < 0)  //Go up
        {
            startX = (int)previousChunkPosition.x - chunkSize * 2;
            startY = (int)previousChunkPosition.y - chunkSize * 2;

            endX = startX + chunkSize * (1 + 3 + 1);
            endY = startY + chunkSize * 2;
        }
        else if (previousChunkPosition.y - currentChunkPosition.y > 0)  //Go down
        {
            startX = (int)previousChunkPosition.x - chunkSize * 2;
            startY = (int)previousChunkPosition.y + chunkSize;

            endX = startX + chunkSize * (1 + 3 + 1);
            endY = startY + chunkSize * 2;
        }

        for(int y = startY; y <= endY; y++)
        {
            for(int x = startX; x <= endX; x++)
            {
                Vector3 pos = new Vector3(x, y);
                constructionLayer.Unload(pos);
            }
        }*/
    }

    public List<Vector2> GetChunksAroundPlayer()
    {
        return chunksAroundPlayer;
    }
}

public class ChunkObject
{
    public GameObject gameObject;

    public Vector2 position;
}
