using UnityEngine;

class PlayerInput : MonoBehaviour
{
    public delegate void OnPlayerInputDelegate(InputInfo info);
    public static event OnPlayerInputDelegate OnPlayerInput;
    [SerializeField] float doubleClickTime = 0.3f;
    [SerializeField] int clicks;
    private bool triggered;
    private float time;
    private void Update()
    {
        if (time > 0 && clicks >= 2 && !triggered)
        {
            OnDoubleClick();
            triggered = true;
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            clicks++;
            if (clicks == 1 && time <= 0)
            {
                OnSingleClick();

            }
            time = doubleClickTime;
        }
        else if (time < 0)
        {
            clicks = 0;
            triggered = false;

        }
        time -= Time.deltaTime;
    }

    private void OnSingleClick()
    {
        Vector2 origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D ray = Physics2D.Raycast(origin, Vector2.zero, float.MaxValue, LayerMask.GetMask("Player"));
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (ray.collider != null)
            {
                if (ray.collider.TryGetComponent(out ISelectable selectable))
                {
                    selectable.OnSelect();
                }
            }

        }
        else
        {
            OnPlayerInput?.Invoke(new InputInfo(new Vector2(), InputInfo.CommandType.Deselect));
        }

    }
    private void OnDoubleClick()
    {
        Debug.Log("On double click");
        OnPlayerInput?.Invoke(new InputInfo(Camera.main.ScreenToWorldPoint(Input.mousePosition), InputInfo.CommandType.Move));
    }
}
