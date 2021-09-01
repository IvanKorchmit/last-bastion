using UnityEngine;

[CreateAssetMenu(fileName = "New Melee", menuName = "Weapons/Melee")]
public class Melee : Weapon
{
    [SerializeField] private float meleeDamage;
    public override void Use(GameObject owner, Transform target)
    {
        if (target != null)
        {
            target.GetComponent<IDamagable>().Damage(meleeDamage);
        }
    }
}
