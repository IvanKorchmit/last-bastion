using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.right * speed;
    }
}
