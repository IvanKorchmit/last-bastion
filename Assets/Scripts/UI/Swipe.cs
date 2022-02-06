using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;
public class Swipe : MonoBehaviour
{
    [SerializeField] private float sensitivity;
    [SerializeField] private float deceleration = 1.5f;
    [SerializeField] private float scroll;
    [SerializeField] private bool isInCanvas = false;
    [SerializeField] private bool isClamped;
    [SerializeField] private Transform bottomLeft;
    [SerializeField] private Transform topRight;

    public float zoomOutMin = 8;
    public float zoomOutMax = 256;
    private Camera self;
    private PixelPerfectCamera ppCam;
    private Vector2 previousPos;
    public Vector2 MouseDelta
    {
        get
        {
#if UNITY_ANDROID
            Vector2 currentMouse = new Vector2();
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                currentMouse = isInCanvas ? touch.position : (Vector2)self.ScreenToWorldPoint(touch.position);
            }
#else
            Vector2 currentMouse = !isInCanvas ? self.ScreenToWorldPoint(Input.mousePosition) : Input.mousePosition;
#endif
            return currentMouse - previousPos;

        }
    }
    private void Start()
    {
        self = Camera.main;
        ppCam = self.GetComponent<PixelPerfectCamera>();
    }
    void Update()
    {
#if !UNITY_ANDROID
        if (Input.GetMouseButtonDown(0))
#else 
        if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
#endif
        {
#if !UNITY_ANDROID
            previousPos = !isInCanvas ? self.ScreenToWorldPoint(Input.mousePosition) : Input.mousePosition;
#else
                previousPos = !isInCanvas ? (Vector2)self.ScreenToWorldPoint(Input.GetTouch(0).position) : Input.GetTouch(0).position;
#endif
        }
#if !UNITY_ANDROID
        if (Input.GetMouseButton(0))
        {
#else
        if (Input.touchCount == 1)
        {
#endif
            Vector2 delta = !isInCanvas ? -MouseDelta : MouseDelta;
            transform.Translate(delta.x * Time.deltaTime * sensitivity, delta.y * Time.deltaTime * sensitivity, 0);
        }
#if !UNITY_ANDROID
        else if (Input.GetMouseButtonUp(0))
#else
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
#endif
        {
            previousPos = !isInCanvas ? -MouseDelta : MouseDelta;

        }
#if !UNITY_ANDROID
        else
        {
#else
        else if (Input.touchCount == 0)
        {
#endif
            previousPos = Vector2.Lerp(previousPos, Vector2.zero, Time.deltaTime * deceleration);
        }
        if (!isInCanvas)
        {
#if UNITY_ANDROID
            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                float difference = currentMagnitude - prevMagnitude;

                Zoom(difference * scroll);
            }
#else
            ppCam.assetsPPU += Mathf.RoundToInt((Input.mouseScrollDelta.y * scroll) / 2) * 2;
            ppCam.assetsPPU = Mathf.Clamp(ppCam.assetsPPU, (int)zoomOutMin, (int)zoomOutMax);
#endif

        }
#if !UNITY_ANDROID
        if (!Input.GetMouseButton(0))
        {
#else
        if (Input.touchCount == 0)
        {
#endif
            transform.Translate(previousPos.x * Time.deltaTime * sensitivity, previousPos.y * Time.deltaTime * sensitivity, 0);
        }
        if (isClamped)
        {
            ClampPosition();
        }
    }
#if UNITY_ANDROID

    void Zoom(float increment)
    {
        ppCam.assetsPPU = Mathf.RoundToInt(Mathf.Clamp((float)ppCam.assetsPPU + increment, zoomOutMin, zoomOutMax) / 2) * 2;
    }
#endif
    private void ClampPosition()
    {
        float z = transform.position.z;
        Vector3 clamped = Vector2Utils.Clamp(transform.position, bottomLeft.position, topRight.position);
        clamped.z = z;
        transform.position = clamped;
        
    }
}
