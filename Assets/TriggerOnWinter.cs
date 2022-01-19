using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerOnWinter : MonoBehaviour
{
    private Animator animator;
    private bool isCurrentlyWinter;
    void Start()
    {
        animator = GetComponent<Animator>();
        Calendar.OnWinter_Property += Calendar_OnWinter;
    }

    private void Calendar_OnWinter(bool isWinter)
    {
        if (isWinter != isCurrentlyWinter)
        {
            animator.SetBool("isWinter", isWinter);
            isCurrentlyWinter = isWinter;
        }
    }
}
