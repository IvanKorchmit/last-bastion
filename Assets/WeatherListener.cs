using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LastBastion.Waves;

public class WeatherListener : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;
    private ParticleSystem ps;
    private AudioSource rainSound;
    private bool isRaining;
    void Start()
    {
        animator = GetComponent<Animator>();
        ps = GetComponent<ParticleSystem>();
        AcidRain.OnAcidRainChange += AcidRain_OnAcidRainChange;
        if (ps == null)
        {
            WavesUtils.OnDayChanged += WavesUtils_OnDayChanged;
        }
        else
        {
            ps.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
            rainSound = GetComponent<AudioSource>();
        }
        if (CompareTag("SnowParticle"))
        {
            Calendar.OnWinter_Property += Calendar_OnWinter_Property;
        }
        
    }

    private void Calendar_OnWinter_Property(bool isWinter)
    {
        if (isWinter)
        {
            ps.Play();
        }
        else
        {
            ps.Stop();
        }
    }

    private void WavesUtils_OnDayChanged(WavesUtils.DayTime obj)
    {
        animator.SetBool("isDay", obj == WavesUtils.DayTime.Day);
    }

    private void AcidRain_OnAcidRainChange(AcidRain.AcidRainStatusType info)
    {
        isRaining = info == AcidRain.AcidRainStatusType.Begin;
        if (ps == null)
        {
            animator.SetBool("isAcid", info == AcidRain.AcidRainStatusType.Begin);
        }
        else
        {
            switch (info)
            {
                case AcidRain.AcidRainStatusType.Begin:
                    ps.Play();
                    break;
                case AcidRain.AcidRainStatusType.End:
                    ps.Stop();
                    break;
            }
        }
    }
    private void Update()
    {
        if (rainSound != null)
        {
            if (isRaining && rainSound.volume < 1f)
            {
                rainSound.volume += Time.deltaTime;
            }
            else if (!isRaining && rainSound.volume > 0f)
            {
                rainSound.volume -= Time.deltaTime;
            }
        }
    }
}
