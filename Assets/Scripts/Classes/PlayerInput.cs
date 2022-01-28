using UnityEngine;

class PlayerInput : MonoBehaviour
{
    public delegate void OnPlayerInputDelegate(InputInfo info);
    public static event OnPlayerInputDelegate OnPlayerInput;
    [SerializeField] float doubleClickTime = 0.3f;
    [SerializeField] int clicks;
    private bool triggered;
    private float time;



    private float timePressed = 0.0f;
    private float timeLastPress = 0.0f;
    public float timeDelayThreshold = 0.25f;


    void checkForLongPress(float tim)
    {
        if (Input.touchCount == 0) return;
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        { // If the user puts her finger on screen...
            timePressed = Time.time - timeLastPress;
            if (timePressed > tim)
            {
                Vector2 origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                RaycastHit2D rayEnemy = Physics2D.CircleCast(origin, 3f, Vector2.zero, float.MaxValue, LayerMask.GetMask("Enemy"));
                if (rayEnemy.collider != null && rayEnemy.collider.CompareTag("Enemy"))
                {
                    OnPlayerInput?.Invoke(new InputInfo(new Vector2(), InputInfo.CommandType.Follow));
                    return;
                }

            }
        }

        if (Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            if (Input.GetTouch(0).tapCount == 2)
            {
                timeLastPress = Time.time;
                if (timePressed > tim)
                {
                    Vector2 origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    RaycastHit2D ray = Physics2D.CircleCast(origin, 3f, Vector2.zero, float.MaxValue, LayerMask.GetMask("Player"));
                    if (ray.collider != null)
                    {
                        if (ray.collider.TryGetComponent(out ISelectable selectable))
                        {
                            selectable.OnSelect();
                        }
                    }
                    OnPlayerInput?.Invoke(new InputInfo(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), InputInfo.CommandType.Move));
                }
            }
            else
            {
                OnPlayerInput?.Invoke(new InputInfo(new Vector2(), InputInfo.CommandType.Deselect));
            }
        }
    }




    private void Update()
    {
        checkForLongPress(timeDelayThreshold);
#if !UNITY_ANDROID
        if (Input.GetKeyDown(KeyCode.F))
        {
            OnPlayerInput?.Invoke(new InputInfo(Camera.main.ScreenToWorldPoint(Input.mousePosition), InputInfo.CommandType.Follow));
        }
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
#endif
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
        OnPlayerInput?.Invoke(new InputInfo(Camera.main.ScreenToWorldPoint(Input.mousePosition), InputInfo.CommandType.Move));
    }
}
