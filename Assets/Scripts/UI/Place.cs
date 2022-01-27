using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Pathfinding;
using LastBastion.Waves;
public class Place : MonoBehaviour
{
    public static event System.Action<PurchaseInfo> OnPlacecd;
    private static Good goodToPlace;
    private static PurchaseInfo lastPurchase;
    private void Start()
    {
        Shop_BuyButton.OnGoodSelection += Shop_BuyButton_OnGoodSelection;
    }

    private void Shop_BuyButton_OnGoodSelection(PurchaseInfo info)
    {
        if (info.GoodStatus == PurchaseInfo.GoodOperation.Fail) return;
        goodToPlace = info.Good;
        lastPurchase = info;
    }

    private void OnGUI()
    {
        if (Event.current.isMouse && Event.current.clickCount == 2)
        {
            if (goodToPlace != null)
            {
                Vector2 mouse = Input.mousePosition;

                Vector2 pos = Camera.main.ScreenToWorldPoint(mouse);
                pos = Vector2Int.FloorToInt(pos);
                var entity = Instantiate(goodToPlace.Prefab, goodToPlace.IsUnit ? GameObject.Find(WavesUtils.COLONY_PATH).transform.position : (Vector3)pos, Quaternion.identity);
                
                // Checking if the current good a living Unit?
                if (goodToPlace.IsUnit)
                {
                    entity.GetComponent<UnitAI>().Initialize(pos);
                    HumanResourcesUtils.TakeOne();
                    entity.GetComponent<UnitAI>().Weapon = goodToPlace.Weapon;


                }
                ShopUtils.Buy(goodToPlace);
                goodToPlace = null;
                OnPlacecd?.Invoke(lastPurchase);
            }
        }
    }
}
