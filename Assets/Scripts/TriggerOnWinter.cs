using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LastBastion.TimeSystem;
using LastBastion.TimeSystem.Events;
public class TriggerOnWinter : MonoBehaviour
{
    private Animator animator;
    private static bool isCurrentlyWinter;
    private static bool isCurrentlyFog;
    private static bool isCurrentlyBlizzard;
    private AudioSource blizzard;
    [SerializeField] private bool isFog;
    void Start()
    {
        animator = GetComponent<Animator>();
        if (!isFog)
        {
            Calendar.OnWinter_Property += Calendar_OnWinter;
            Blizzard.OnBlizzard += Blizzard_OnBlizzard;
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

    private void Blizzard_OnBlizzard(bool obj)
    {
        isCurrentlyBlizzard = obj;
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
            if ((isCurrentlyBlizzard || isCurrentlyWinter) && blizzard.volume < 1f)
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
