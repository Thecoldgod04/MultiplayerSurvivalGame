using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ChunkBorderDirection
{
    Up,
    Right,
    Down,
    Left
}
public class ChunkBorder : MonoBehaviour
{
    [SerializeField]
    private ChunkBorderDirection chunkBorderDirection;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }
}