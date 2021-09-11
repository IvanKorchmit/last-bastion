using UnityEngine;

[CreateAssetMenu(fileName = "New Melee", menuName = "Weapons/Melee")]
public class Melee : Weapon
{
    [SerializeField] private float meleeDamage;
    [SerializeField] private float range;
    [SerializeField] private float cooldown;
    public float Range => range;
    public float MeleeDamage => meleeDamage;
    public float Cooldown => cooldown;
    public override void Use(GameObject owner, Transform target)
    {
        if (target != null)
        {
            target.GetComponent<IDamagable>().Damage(meleeDamage, owner);
        }
    }
}
