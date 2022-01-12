using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
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
            transform.Translate(delta.x / sensitivity, delta.y / sensitivity, 0);
        }
        if (!isInCanvas)
        {
            self.orthographicSize += Input.mouseScrollDelta.y * sensitivity;
        }
        previousPos = !isInCanvas ? self.ScreenToWorldPoint(Input.mousePosition) : Input.mousePosition;
    }
}
