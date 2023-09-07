using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldConfiguration : MonoBehaviour
{
    public static WorldConfiguration instance;
    //public static const Dictionary<string, int>
    [field: SerializeField]
    public int WorldSize { get; private set; }

    public BiomeConfig BiomeConfig { get; private set; }
    public EnvironmentConfig EnvironmentConfig { get; private set; }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;

            WorldSize = 300;

            BiomeConfig = new BiomeConfig();
            EnvironmentConfig = new EnvironmentConfig();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}

public class BiomeConfig
{
    //Biome codes
    public int ocean = 0;
    public int plains = 1;
    public int desert = 2;
    public int snowy = 3;
    public int jungle = 4;
    public int beach = 5;
}
public class EnvironmentConfig
{
    //Environment codes
    public int air = 0;
    public int cactus = 3;
    public int smallTree = 4;
    public int smallgrass = 5;
    public int bigTree = 6;
    public int tallGrass = 7;

    public Dictionary<int, Dictionary<int, float>> environmentConfig = new Dictionary<int, Dictionary<int, float>>()
    {
        {
            WorldConfiguration.instance.BiomeConfig.plains, new Dictionary<int, float>()
            {
                {4, 0.1f},
                //{5, 0.25f},
                /*{4, 0.2f},
                {5, 0.1f}*/
            }
        },

        {
            WorldConfiguration.instance.BiomeConfig.desert, new Dictionary<int, float>()
            {
                {3, 0.018f},
            }
        }
    };
}
