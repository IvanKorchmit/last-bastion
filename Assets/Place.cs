using UnityEngine;

public class Place : MonoBehaviour
{
    private void Start()
    {
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
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
