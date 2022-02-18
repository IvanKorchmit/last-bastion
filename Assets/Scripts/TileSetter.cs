using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSetter : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private RandomTile[] randTile;
    [SerializeField] private RandomSizeTile[] randSizedTile;
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
                foreach (RandomSizeTile t in randSizedTile)
                {
                    if (t.IsMatch((Tile)tilemap.GetTile(pos)))
                    {
                        tilemap.SetTile(pos, t.GetTIle((Vector2Int)pos));
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
[System.Serializable]
public struct RandomSizeTile
{
    [SerializeField] private Tile from;
    [SerializeField] private RandomLargeTile[] to;
    [SerializeField] private Vector2Int size;
    public bool IsMatch(Tile from)
    {
        return this.from == from;
    }
    public Tile GetTIle(Vector2Int position)
    {
        Vector2Int mod = Vector2Utils.IntMod(position, size);
        return to[Random.Range(0, to.Length)].GetTIle(position, size);

    }
}
[System.Serializable]
public struct RandomLargeTile
{
    [SerializeField] private Tile[] tiles;
    public Tile GetTIle(Vector2Int position, Vector2Int size)
    {
        Vector2Int mod = new Vector2Int(Mathf.Abs(position.x % size.x) /*+ (position.y % size.y == 1 ? 1 : 0)*/, Mathf.Abs(position.y % size.y));
        int index = Mathf.Abs(mod.x * size.x + mod.y);
        return tiles[index];
    }
}