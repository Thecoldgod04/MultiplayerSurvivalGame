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

            WorldSize = 100;

            BiomeConfig = new BiomeConfig();
            EnvironmentConfig = new EnvironmentConfig();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}

[System.Serializable]
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
    //Environment codes (these must match the index in the ItemMetaManager)
    public int air = 0;
    public int cactus = 3;
    public int smallTree = 4;
    public int bigTree = 5;
    public int smallgrass = 6;
    public int tallGrass = 7;
    public int skull = 8;
    public int crate = 9;

    public Dictionary<int, Dictionary<int, float>> environmentConfig = new Dictionary<int, Dictionary<int, float>>()
    {
        {
            WorldConfiguration.instance.BiomeConfig.plains, new Dictionary<int, float>()
            {
                {4, 0.01f},      //small tree
                {5, 0.005f},     //big tree
                {6, 0.1f},      //small grass
                {7, 0.08f},      //tall grass


                {9, 0.001f},    //crate
                //{7, 0.04f}
            }
        },

        {
            WorldConfiguration.instance.BiomeConfig.desert, new Dictionary<int, float>()
            {
                {3, 0.018f},    //cactus
                {8, 0.01f},     //skull

                {9, 0.001f},    //crate
            }
        },

        {
            WorldConfiguration.instance.BiomeConfig.jungle, new Dictionary<int, float>()
            {
                {4, 0.04f},     //small tree
                {5, 0.02f},     //big tree
                {6, 0.2f},      //small grass
                {7, 0.1f},      //tall grass


                {9, 0.001f},    //crate
            }
        },
    };
}

/*[System.Serializable]
public struct EnvironmentConfig
{
    
}*/ 
