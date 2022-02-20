using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.EventSystems;
public class Swipe : MonoBehaviour
{
    [SerializeField] private float sensitivity;
    [SerializeField] private float deceleration = 1.5f;
    [SerializeField] private bool isInCanvas = false;
    [SerializeField] private bool isClamped;
    [SerializeField] private Transform bottomLeft;
    [SerializeField] private Transform topRight;
    private Camera self;
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
    }
    void Update()
    {
#if !UNITY_ANDROID
        if (Input.GetMouseButtonDown(0) && (!isInCanvas ? !EventSystem.current.IsPointerOverGameObject() : true))
#else 
        if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began && (!isInCanvas ? !EventSystem.current.IsPointerOverGameObject() : true))
#endif
        {
#if !UNITY_ANDROID
            previousPos = !isInCanvas ? self.ScreenToWorldPoint(Input.mousePosition) : Input.mousePosition;
#else
                previousPos = !isInCanvas ? (Vector2)self.ScreenToWorldPoint(Input.GetTouch(0).position) : Input.GetTouch(0).position;
#endif
        }
#if !UNITY_ANDROID
        if (Input.GetMouseButton(0) && (!isInCanvas ? !EventSystem.current.IsPointerOverGameObject() : true))
        {
#else
        if (Input.touchCount == 1 && (!isInCanvas ? !EventSystem.current.IsPointerOverGameObject() : true))
        {
#endif
            Vector2 delta = !isInCanvas ? -MouseDelta : MouseDelta;
            transform.Translate(delta.x * Time.deltaTime * sensitivity, delta.y * Time.deltaTime * sensitivity, 0);
        }
#if !UNITY_ANDROID
        else if (Input.GetMouseButtonUp(0) && (!isInCanvas ? !EventSystem.current.IsPointerOverGameObject() : true))
#else
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended && (!isInCanvas ? !EventSystem.current.IsPointerOverGameObject() : true))
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
    private void ClampPosition()
    {
        float z = transform.position.z;
        Vector3 clamped = Vector2Utils.Clamp(transform.position, bottomLeft.position, topRight.position);
        clamped.z = z;
        transform.position = clamped;

    }
}
