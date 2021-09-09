using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Swipe : MonoBehaviour
{
    [SerializeField] private float sensitivity;
    private Camera self;
    private Vector2 previousPos;
    public Vector2 MouseDelta
    {
        get
        {
            Vector2 currentMouse = self.ScreenToWorldPoint(Input.mousePosition);
            return previousPos - currentMouse;
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
        previousPos = self.ScreenToWorldPoint(Input.mousePosition);
    }
}
