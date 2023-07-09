using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TilemapLayer : MonoBehaviour
{
    [field: SerializeField]
    public Tilemap tilemap { get; private set; }

    protected virtual void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }
}
