using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Photon.Pun;

[RequireComponent(typeof(Tilemap))]
public class TilemapLayer : MonoBehaviourPun
{
    [field: SerializeField]
    public Tilemap tilemap { get; private set; }

    protected virtual void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }
}
