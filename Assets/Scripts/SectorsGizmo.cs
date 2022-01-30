using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorsGizmo : MonoBehaviour
{
    private static List<SpriteRenderer> sectors;
    private static Transform origin;
    [SerializeField] private GameObject sectorPrefab;
    [SerializeField] private static GameObject staticPrefab;
    private void Start()
    {
        origin = transform;
        sectors = new List<SpriteRenderer>(Sectors.GridSize.x * Sectors.GridSize.y);
        staticPrefab = sectorPrefab;
        Place.OnPlacecd += Place_OnPlacecd;
    }

    private void Place_OnPlacecd(PurchaseInfo obj)
    {
        RemoveSectors();
    }

    private void OnDrawGizmos()
    {
        for (int x = 0; x < Sectors.GridSize.x; x++)
        {
            for (int y = 0; y < Sectors.GridSize.y; y++)
            {
                Vector2 o = (Vector2)transform.position;
                Vector2 i = new Vector2(x,y) * Sectors.SECTOR_SIZE;
                Vector2 pos = o + i;
                Gizmos.color = Sectors.HasSomething(new Vector2Int(x,y)) ? Color.green : Color.red;
                Gizmos.DrawWireCube(pos, new Vector3(Sectors.SECTOR_SIZE, Sectors.SECTOR_SIZE, 1));
            }
        }
    }
    private void FixedUpdate()
    {
        if (sectors.Count > 0)
        {
            for (int x = 0; x < Sectors.GridSize.x; x++)
            {
                for (int y = 0; y < Sectors.GridSize.y  ; y++)
                {
                    int index = x * Sectors.GridSize.y + y;
                    SpriteRenderer sec = sectors[index];
                    sec.color = Sectors.HasSomething(new Vector2Int(x, y)) ? Color.red : Color.green;
                }
            }
        }
    }
    public static void DrawSectors()
    {
        for (int x = 0; x < Sectors.GridSize.x; x++)
        {
            for (int y = 0; y < Sectors.GridSize.y; y++)
            {
                Vector2 o = (Vector2)origin.position;
                Vector2 i = new Vector2(x, y) * Sectors.SECTOR_SIZE;
                Vector2 pos = o + i;
                var sec = Instantiate(staticPrefab, pos, Quaternion.identity).GetComponent<SpriteRenderer>();
                sec.color = Sectors.HasSomething(new Vector2Int(x, y)) ? Color.red : Color.green;
                sectors.Add(sec);
            }
        }
    }
    private static void RemoveSectors()
    {
        while (sectors.Count > 0)
        {
            Destroy(sectors[0].gameObject);
            sectors.RemoveAt(0);
        }
    }
}
