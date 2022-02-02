using UnityEngine;

[System.Serializable]
public class Good
{
    [SerializeField] private bool isUnit;
    [SerializeField] private int cost;
    [SerializeField] private GameObject prefab;
    [SerializeField] private WeaponBase weapon;
    public int Cost => cost;
    public GameObject Prefab => prefab;
    public WeaponBase Weapon => weapon; 
    public bool IsUnit => isUnit;
}
