using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;
public class Swipe : MonoBehaviour
{
    [SerializeField] private float sensitivity;
    [SerializeField] private bool isInCanvas = false;
    private Camera self;
    private PixelPerfectCamera ppCam;
    private Vector2 previousPos;
    public Vector2 MouseDelta
    {
        get
        {
            Vector2 currentMouse = !isInCanvas ? self.ScreenToWorldPoint(Input.mousePosition) : Input.mousePosition;
            return !isInCanvas ? previousPos - currentMouse : currentMouse - previousPos;
        }
    }
    private void Start()
    {
        self = Camera.main;
        ppCam = self.GetComponent<PixelPerfectCamera>();
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 delta = MouseDelta;
            transform.Translate(delta.x * sensitivity * Time.deltaTime, delta.y * sensitivity * Time.deltaTime, 0);
        }
        if (!isInCanvas)
        {
            ppCam.assetsPPU += Mathf.RoundToInt((Input.mouseScrollDelta.y * sensitivity * 2) / 4) * 4;
            ppCam.assetsPPU = Mathf.Clamp(ppCam.assetsPPU, 8, 256);
        }
        previousPos = Vector2.Lerp(previousPos,!isInCanvas ? self.ScreenToWorldPoint(Input.mousePosition) : Input.mousePosition,0.5f);
    }
}
