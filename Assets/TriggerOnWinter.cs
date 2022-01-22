using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerOnWinter : MonoBehaviour
{
    private Animator animator;
    private bool isCurrentlyWinter;
    private bool isCurrentlyFog;
    private AudioSource blizzard;
    [SerializeField] private bool isFog;
    void Start()
    {
        animator = GetComponent<Animator>();
        if (!isFog)
        {
            Calendar.OnWinter_Property += Calendar_OnWinter;
            if (CompareTag("MainCamera"))
            {
                blizzard = GetComponent<AudioSource>();
            }
        }
        else
        {
            Calendar.OnFog += Calendar_OnFog;
        }
    }

    private void Calendar_OnFog(bool isFog)
    {
        if (isFog != isCurrentlyFog)
        {
            animator.SetBool("isFog", isFog);
            isCurrentlyFog = isFog;
        }
    }

    private void Calendar_OnWinter(bool isWinter)
    {
        if (isWinter != isCurrentlyWinter)
        {
            animator.SetBool("isWinter", isWinter);
            isCurrentlyWinter = isWinter;
        }
    }
    private void Update()
    {
        if (blizzard != null)
        {
            if (isCurrentlyWinter && blizzard.volume < 1f)
            {
                blizzard.volume += Time.deltaTime;
            }
            else if (!isCurrentlyWinter && blizzard.volume > 0f)
            {
                blizzard.volume -= Time.deltaTime;
            }
        }
    }
}
