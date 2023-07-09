using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New BuildableMeta", menuName = "BuildableMeta")]
public class BuildableMeta : ItemMeta
{
    [field: SerializeField]
    public TileBase tile { get; private set; }

    [field: SerializeField]
    public GameObject gameObject { get; private set; }

    [field: SerializeField]
    public BuildableType type { get; private set; }

    private TileBase transparentTile;

    public override void Init(int id)
    {
        base.Init(id);
        transparentTile = Resources.Load<TileBase>("Tile_Palette/Transparent_Tile");
        SetUp();   
    }

    public BuildableType GetBuildableType()
    {
        return type;
    }

    private void SetUp()
    {
        RegisterType();
        if(type == BuildableType.GameObject)
        {
            tile = transparentTile;
        }
    }

    private void RegisterType()
    {
        if (gameObject != null && tile != null && tile != transparentTile)
        {
            Debug.LogError("The buildable_meta can not be both GameObject and Tile from buildable_meta ID: " + GetId());
            type = BuildableType.None;
        }    
        else if(gameObject == null && tile == null)
        {
            Debug.LogError("The buildable_meta fields can not be null!");
            type = BuildableType.None;
        }
        else if (gameObject != null)
        {
            type = BuildableType.GameObject;
        }
        else
        {
            type = BuildableType.Tile;
        }
    }
}

public enum BuildableType
{
    None,
    Tile,
    GameObject
}
