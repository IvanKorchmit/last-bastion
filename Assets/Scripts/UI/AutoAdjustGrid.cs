using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AutoAdjustGrid : MonoBehaviour
{
    private GridLayoutGroup grid;
    private void Start()
    {
        grid = GetComponent<GridLayoutGroup>();
        RectTransform r = transform as RectTransform;
        Vector2 cellSize = new Vector2(0,100);
        cellSize.x = (r.rect.width + grid.padding.left) / transform.childCount;
        foreach (RectTransform t in transform)
        {
            t.localScale = Vector3.one;
        }
        grid.cellSize = cellSize;
        
    }
    private void OnValidate()
    {
        Start();
    }
}
