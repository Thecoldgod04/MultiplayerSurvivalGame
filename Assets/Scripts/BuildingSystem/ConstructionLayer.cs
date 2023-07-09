using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ConstructionLayer : TilemapLayer
{
    public Dictionary<Vector3Int, BuildableObject> occupiedList { get; private set; }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        occupiedList = new Dictionary<Vector3Int, BuildableObject>();
    }

    public void Build(Vector3 coords, BuildableMeta buildableMeta)
    {
        if (IsEmpty(coords) == false) return;
        if (buildableMeta.GetBuildableType() == BuildableType.None) return;
        Vector3Int cellCoords = tilemap.WorldToCell(coords);

        GameObject gameObject = null;
        if(buildableMeta.GetBuildableType() == BuildableType.GameObject)
        {
            Vector3 worldCoord = tilemap.CellToWorld(cellCoords);
            worldCoord.x += 0.5f;
            worldCoord.y += 0.5f;
            worldCoord.z = -0.1f;
            gameObject = Instantiate(buildableMeta.gameObject, worldCoord, Quaternion.identity);
        }
        tilemap.SetTile(cellCoords, buildableMeta.tile);

        BuildableObject buildableObject = new BuildableObject(buildableMeta, gameObject);

        occupiedList.Add(cellCoords, buildableObject);
    }
    public void Destroy(Vector3 position)
    { 
        Vector3Int cellCoord = tilemap.WorldToCell(position);

        if (IsEmpty(cellCoord)) return;
        if(occupiedList.ContainsKey(cellCoord))
        {
            if (occupiedList[cellCoord].realGameObject != null)
            {
                Destroy(occupiedList[cellCoord].realGameObject);
            }
        }
        tilemap.SetTile(cellCoord, null);

        occupiedList.Remove(cellCoord);
    }

    public bool IsEmpty(Vector3 coords)
    {
        Vector3Int cellCoords = tilemap.WorldToCell(coords);
        return occupiedList.ContainsKey(cellCoords) == false && tilemap.GetTile(cellCoords) == null;
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
