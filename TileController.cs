using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileController : MonoBehaviour {

    public TileBase tile;
    public Vector3Int position;
    public Tilemap tilemap;
    [ContextMenu("Paint")]
    void Paint() {
        tilemap.SetTile(position, tile);
    }

}
