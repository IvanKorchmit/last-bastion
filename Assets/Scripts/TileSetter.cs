using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSetter : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private RandomTile[] randTile;
    private void Start()
    {
        Vector3Int offset = tilemap.origin;
        for (int x = 0; x < tilemap.size.x; x++)
        {
            for (int y = 0; y < tilemap.size.y; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0) + offset;
                foreach (RandomTile t in randTile)
                {
                    if (t.IsMatch((Tile)tilemap.GetTile(pos)))
                    {
                        tilemap.SetTile(pos, t.Tile);
                        break;
                    }
                }
            }
        }
    }

}
[System.Serializable] 
public struct RandomTile
{
    [SerializeField] private Tile from;
    [SerializeField] private Tile[] to;
    public bool IsMatch(Tile from)
    {
        return this.from == from;
    }
    public Tile @Tile => to[Random.Range(0, to.Length)];

}
