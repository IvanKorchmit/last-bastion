using UnityEngine;

public class Place : MonoBehaviour
{
    private void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary && Input.GetTouch(0).phase != TouchPhase.Moved)
        {
            if (Placement.objectToPlace != null)
            {
                ShopUtils.Buy(Placement.objectToPlace.Cost, Placement.objectToPlace);
                Placement.objectToPlace = null;
                ShopUtils.UIPanel_Reference.shopWindow.gameObject.SetActive(true);
            }
        }
    }
}
