using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Swipe : MonoBehaviour
{
    [SerializeField] private float sensitivity;
    [SerializeField] private bool isInCanvas = false;
    private Camera self;
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
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 delta = MouseDelta;
            transform.Translate(delta.x / sensitivity, delta.y / sensitivity,0);
        }
        previousPos = !isInCanvas ? self.ScreenToWorldPoint(Input.mousePosition) : Input.mousePosition;
    }
}
