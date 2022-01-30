using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;
public class Swipe : MonoBehaviour
{
    [SerializeField] private float sensitivity;
    [SerializeField] private float scroll;
    [SerializeField] private bool isInCanvas = false;
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
            if (Input.touchCount > 0 )
            {
                Touch touch = Input.GetTouch(0);
                currentMouse = isInCanvas ? touch.deltaPosition : -touch.deltaPosition / ppCam.assetsPPU;
                // currentMouse /= screen;
            }
            else
            {
                return new Vector2();
            }
#else
            Vector2 currentMouse = !isInCanvas ? self.ScreenToWorldPoint(Input.mousePosition) : Input.mousePosition;
#endif
#if !UNITY_ANDROID
            return -currentMouse - previousPos;

#else
            return currentMouse;
#endif
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
        if (Input.GetMouseButton(0))
        {
#endif
            Vector2 delta = MouseDelta;
            transform.Translate(delta.x * Time.deltaTime * sensitivity, delta.y * Time.deltaTime * sensitivity, 0);
            previousPos += delta;

#if !UNITY_ANDROID
        }
        else
        {
            previousPos += MouseDelta;
        }
#endif
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

                zoom(difference * scroll);
            }




#else
            ppCam.assetsPPU += Mathf.RoundToInt((Input.mouseScrollDelta.y * scroll) / 4) * 4;
            ppCam.assetsPPU = Mathf.Clamp(ppCam.assetsPPU, 8, 256);
#endif
            //previousPos = Vector2.Lerp(previousPos,delta,0.5f);
        }
    }
#if UNITY_ANDROID

    void zoom(float increment)
    {
        ppCam.assetsPPU = Mathf.RoundToInt(Mathf.Clamp((float)ppCam.assetsPPU + increment, zoomOutMin, zoomOutMax) / 4) * 4;
    }
#endif
}
