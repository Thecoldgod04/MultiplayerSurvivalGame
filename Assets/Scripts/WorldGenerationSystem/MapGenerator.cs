using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

public class MapGenerator : MonoBehaviourPun
{
    [SerializeField]
    private int width, height;

    private int seed;

    [SerializeField]
    private float scale;

    [field: SerializeField]
    private NoiseGenerator noiseGenerator;

    private float[,] temperature, altitude, moisture;

    [SerializeField]
    private GameObject[] testTiles;

    [SerializeField]
    private BiomeLayer biomeLayer;

    [SerializeField]
    private ConstructionLayer constructionLayer;

    [SerializeField]
    private EnvironmentGenerator environmentGenerator;

    public int[,] biomeData { get; private set; }
    public int[,] environmentData { get; private set; }

    private int worldSize;
    private WorldConfiguration worldConfiguration;

    // Start is called before the first frame update
    void Start()
    {
        noiseGenerator = new NoiseGenerator();
        worldConfiguration = WorldConfiguration.instance;

        if (PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Joined && PhotonNetwork.IsMasterClient)
        {
            seed = Random.Range(0, 2);

            ExitGames.Client.Photon.Hashtable customPropWorldSeed = new ExitGames.Client.Photon.Hashtable();
            customPropWorldSeed["world_seed"] = seed;
            PhotonNetwork.CurrentRoom.SetCustomProperties(customPropWorldSeed);
        }
        else if(PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Joined && !PhotonNetwork.IsMasterClient)
        {
            seed = (int) PhotonNetwork.CurrentRoom.CustomProperties["world_seed"];
            Logger.LogError("MapGenerator", "Seed: " + seed);
        }
        else
        {
            seed = 0;
        }

        System.Random proceduralRandomizer = new System.Random(seed);
        float proceduralRandomizedAlt = proceduralRandomizer.Next(-100000, 10000);
        float proceduralRandomizedTemp = proceduralRandomizer.Next(-100000, 10000);
        float proceduralRandomizedMoist = proceduralRandomizer.Next(-100000, 10000);

        Debug.LogError("ALT: " + proceduralRandomizedAlt);
        Debug.LogError("TEMP: " + proceduralRandomizedTemp);
        Debug.LogError("MOIST: " + proceduralRandomizedMoist);

        worldSize = WorldConfiguration.instance.WorldSize;

        temperature = noiseGenerator.GetNoiseMap(worldSize, worldSize, proceduralRandomizedTemp, scale);
        altitude = noiseGenerator.GetNoiseMap(worldSize, worldSize, proceduralRandomizedAlt, scale);
        moisture = noiseGenerator.GetNoiseMap(worldSize, worldSize, proceduralRandomizedMoist, scale);

        biomeData = GetBiomeData();
        environmentData = GetEnvironmentData(biomeData);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private int[,] GetBiomeData()
    {
        int[,] biomeData = new int[worldSize, worldSize];

        for (int y = 0; y < worldSize; y++)
        {
            for (int x = 0; x < worldSize; x++)
            {
                Vector2 pos = new Vector2(x, y);
                float alt = altitude[x, y];
                float temp = temperature[x, y];
                float moist = moisture[x, y];

                //Ocean
                if (alt < 0.4f)
                {
                    //Instantiate(testTiles[0], pos, Quaternion.identity);

                    //biomeLayer.GenerateBiomeBlock("WATER", pos);

                    biomeData[x, y] = WorldConfiguration.instance.BiomeConfig.ocean;
                }
                //Beach
                else if (isBetween(alt, 0.4f, 0.5f))
                {
                    //Instantiate(testTiles[1], pos, Quaternion.identity);

                    //biomeLayer.GenerateBiomeBlock("SAND", pos);

                    biomeData[x, y] = WorldConfiguration.instance.BiomeConfig.beach;
                }
                //Other biomes
                else
                {
                    //Desert
                    if (isBetween(moist, 0f, 0.5f) && isBetween(temp, 0.5f, 1f))
                    {
                        //Instantiate(testTiles[2], pos, Quaternion.identity);

                        //biomeLayer.GenerateBiomeBlock("SAND", pos);

                        biomeData[x, y] = WorldConfiguration.instance.BiomeConfig.desert;
                    }
                    //Jungle
                    else if (isBetween(moist, 0.5f, 1f) && isBetween(temp, 0.5f, 1f))
                    {
                        //Instantiate(testTiles[4], pos, Quaternion.identity);

                        //biomeLayer.GenerateBiomeBlock("GRASS", pos);

                        biomeData[x, y] = WorldConfiguration.instance.BiomeConfig.jungle;
                    }
                    //Snow
                    else if (isBetween(moist, 0f, 0.3f) && isBetween(temp, 0f, 0.5f))
                    {
                        //Instantiate(testTiles[5], pos, Quaternion.identity);

                        //biomeLayer.GenerateBiomeBlock("SNOW", pos);

                        biomeData[x, y] = WorldConfiguration.instance.BiomeConfig.snowy;
                    }
                    //Plains
                    else
                    {
                        //Instantiate(testTiles[3], pos, Quaternion.identity);

                        //biomeLayer.GenerateBiomeBlock("GRASS", pos);

                        biomeData[x, y] = WorldConfiguration.instance.BiomeConfig.plains;
                    }
                }
            }
        }

        biomeLayer.Refresh();
        return biomeData;
    }

    private int[,] GetEnvironmentData(int[,] biomeData)
    {
        int[,] environmentData = new int[worldSize, worldSize];

        Dictionary<int, Dictionary<int, float>> environmentConfig = worldConfiguration.EnvironmentConfig.environmentConfig;

        Random.InitState(seed);

        //if(worldConfiguration.EnvironmentConfig.environmentData)
        for(int y = 0; y < worldSize; y++)
        {
            for(int x = 0; x < worldSize; x++)
            {
                if(environmentConfig.ContainsKey(biomeData[x,y]))
                {
                    foreach(KeyValuePair<int, float> envirConf in environmentConfig[biomeData[x, y]])
                    {
                        if(Random.value < envirConf.Value)
                        {
                            //constructionLayer.Build(new Vector3(x, y), EnvironmentGenerator.cactus);
                            environmentData[x, y] = envirConf.Key;
                            //if (envirConf.Key == 4) Debug.LogWarning("Yaaaa");
                        }
                        else
                        {
                            environmentData[x, y] = worldConfiguration.EnvironmentConfig.air;
                        }
                    }
                }
            }
        }
        //environmentGenerator.GenerateEnv(environmentData);
        return environmentData;
    }

    private bool isBetween(float value, float start, float end)
    {
        if (value >= start && value < end)
            return true;

        return false;
    }
}

[System.Serializable]
class NoiseGenerator
{
    public float[,] GetNoiseMap(int width, int height, float seed, float scale)
    {
        float[,] noiseMap = new float[width, height];

        if (scale <= 0)
        {
            scale = 0.001f;
        }

        //Loop through the whole area of the map and assign a value of perlin noise to each "pixel"
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float sampleX = x / scale;
                float sampleY = y / scale;

                float perlinValue = Mathf.PerlinNoise(sampleX + seed, sampleY + seed);
                noiseMap[x, y] = perlinValue;
            }
        }

        return noiseMap;
    }
}
