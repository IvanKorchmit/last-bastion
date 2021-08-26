using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Swipe : MonoBehaviour
{
    private CanvasScaler canvas;
    [SerializeField] private float sensitivity;
    private Camera self;
    private Vector2 oldPos;
    private void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<CanvasScaler>();
        self = Camera.main;
    }
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            transform.Translate(-t.deltaPosition.x / sensitivity, -t.deltaPosition.y / sensitivity,0);
        }
    }
}
