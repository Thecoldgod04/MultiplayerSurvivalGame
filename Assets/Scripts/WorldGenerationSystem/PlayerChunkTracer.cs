using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerChunkTracer : MonoBehaviourPun
{
    [SerializeField]
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

    [SerializeField]
    private List<Vector2> loadedChunks;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Joined && !photonView.IsMine) return;

        biomeLayer = FindObjectOfType<BiomeLayer>();
        environmentGenerator = FindObjectOfType<EnvironmentGenerator>();
        mapGenerator = FindObjectOfType<MapGenerator>();
        constructionLayer = FindObjectOfType<ConstructionLayer>();

        previousChunkPosition = GetChunkPosition(player.position);
        LoadChunks(previousChunkPosition);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player has moved to a different chunk
        if (PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Joined && !photonView.IsMine) return;

        Vector2 currentChunkPosition = GetChunkPosition(player.position);
        if (currentChunkPosition != previousChunkPosition)  // The player has moved to a different chunk
        {
            // Unload further chunks
            UnloadChunks(previousChunkPosition, currentChunkPosition);

            // Update the previous chunk position
            previousChunkPosition = currentChunkPosition;
            Debug.Log("Player moved to a different chunk: " + currentChunkPosition);

            // Load the nearer chunks
            LoadChunks(currentChunkPosition);
        }
    }

    private Vector2 GetChunkPosition(Vector3 position)
    {
        // Round the position to the nearest multiple of chunkSize
        float x = Mathf.Floor(position.x / chunkSize) * chunkSize;
        float y = Mathf.Floor(position.y / chunkSize) * chunkSize;

        return new Vector2(x, y);
    }

    private void LoadChunks(Vector2 currentChunkPosition)
    {
        int startX = (int) currentChunkPosition.x - chunkSize;
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
        loadedChunks.Add(currentChunkPosition);
    }   
    
    private void UnloadChunks(Vector2 previousChunkPosition, Vector2 currentChunkPosition)
    {
        int startX = 0;
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
        }
    }
}
