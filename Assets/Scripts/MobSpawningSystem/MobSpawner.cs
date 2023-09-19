using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MobSpawner : MonoBehaviourPun
{
    [SerializeField]
    private PlayerChunkTracer playerChunkTracer;

    [SerializeField]
    private int spawnDistance;

    /*[SerializeField]
    private int mobCap;*/

    [SerializeField]
    private float interval;

    [SerializeField]
    private List<GameObject> mobPrefabList;

    // Start is called before the first frame update
    void Start()
    {
        if (!photonView.IsMine) return;

        playerChunkTracer = GetComponent<PlayerChunkTracer>();
        StartCoroutine(SpawnMob(interval, mobPrefabList[0]));
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Joined && !photonView.IsMine) return;

        if(WorldTime.instance.GetCurrentHour() >= 7 && WorldTime.instance.GetCurrentHour() <= 13 && !ReachedMobCap())
        {
            canSpawn = true;
        }
        else
        {
            canSpawn = false;
        }
    }

    bool canSpawn = false;
    private IEnumerator SpawnMob(float interval, GameObject mob)
    {
        yield return new WaitForSeconds(interval);

        if (canSpawn == true)
        {
            //Debug.LogError("Spawn mob!");
            
            GameObject mobObject = null;

            foreach (Vector2 chunk in playerChunkTracer.GetChunksAroundPlayer())
            {
                Vector2 spawnPos = GetSpawnPosition(chunk);
                if (Vector2.Distance(spawnPos, transform.position) >= spawnDistance)
                {
                    mobObject = ObjectPool.instance.GetPooledObject(mob);

                    if (PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.Joined)
                    {
                        if(mobObject == null)
                        {
                            //Debug.LogError("Spawn NEW mob in chunk: " + chunk + " at position: " + spawnPos);
                            mobObject = Instantiate(mob, spawnPos, Quaternion.identity);
                            ObjectPool.instance.AddNewToPool(mobObject);
                        }
                        else
                        {
                            //Debug.LogError("Spawn POOLED mob in chunk: " + chunk + " at position: " + spawnPos);
                            mobObject.transform.position = spawnPos;
                            mobObject.SetActive(true);
                        }
                    }
                    else if (PhotonNetwork.IsMasterClient)
                    {
                        if (mobObject == null)
                        {
                            mobObject = PhotonNetwork.Instantiate(mob.name, spawnPos, Quaternion.identity);
                            //Debug.LogError(mobObject == null);
                            ObjectPool.instance.AddNewToPool(mobObject);
                        }
                        else
                        {
                            mobObject.transform.position = spawnPos;
                            mobObject.SetActive(true);
                        }
                    }
                }
            }
        }
        StartCoroutine(SpawnMob(interval, mob));
    }

    private Vector2 GetSpawnPosition(Vector2 chunkPos)
    {
        //Debug.LogError(chunkPos);
        float maxX = chunkPos.x + ChunkManager.instance.ChunkSize;
        float maxY = chunkPos.y + ChunkManager.instance.ChunkSize;

        float posX = Random.Range(chunkPos.x, maxX+1);
        float posY = Random.Range(chunkPos.y, maxY + 1);

        Vector2 spawnPos = new Vector2(posX, posY);

        //Vector2 spawnPos = new Vector2(maxX, maxY);

        return spawnPos;
    }

    private bool ReachedMobCap()
    {
        return MobManager.instance.ReachedMobCap();
    }
}
