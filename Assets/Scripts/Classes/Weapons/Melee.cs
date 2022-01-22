using UnityEngine;

[CreateAssetMenu(fileName = "New Melee", menuName = "Weapons/Melee")]
public class Melee : WeaponBase
{
    [SerializeField] private float meleeDamage;
    [SerializeField] private float range;
    [SerializeField] private float cooldown;
    [SerializeField] private AudioClip damageSound;
    public AudioClip DamageSound => damageSound;
    public float Range => range;
    public float MeleeDamage => meleeDamage;
    public float Cooldown => cooldown;
    public override void Use(Transform owner, Transform target)
    {
        if (target != null)
        {
            target.GetComponent<IDamagable>().Damage(meleeDamage, owner.root.gameObject);
        }
    }
}
