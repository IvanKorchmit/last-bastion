using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class Place : MonoBehaviour
{
    private void OnGUI()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && Event.current.clickCount >= 2)
        {
            if (Placement.objectToPlace != null)
            {
                if (!Placement.objectToPlace.MustCome || HumanResourcesUtils.TakeOne())
                {
                    ShopUtils.Buy(Placement.objectToPlace.Cost, Placement.objectToPlace);
                    Placement.objectToPlace = null;
                    TimerUtils.AddTimer(0.02f, () => ShopUtils.UIPanel_Reference.shopWindow.gameObject.SetActive(true));
                }
            }
        }
    }
}
