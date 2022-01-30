using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShowBossHealth : MonoBehaviour
{
    private Image healthImage;
    [SerializeField]
    private IDamagable damagable;




    public void Init(IDamagable damagable)
    {
        this.damagable = damagable;
    }

    private void Start()
    {
        healthImage = GetComponent<Image>();
    }

    private void OnGUI()
    {
            healthImage.fillAmount = damagable.Health / damagable.MaxHealth;
    }
}
