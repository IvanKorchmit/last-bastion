using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Weapons/Firearm/Gun")]
public class Firearm : WeaponBase
{
    [SerializeField] private float cooldown;
    [SerializeField] private GameObject bullet;
    [SerializeField] protected float damage;
    private float initialDamage;
    public float InitDamage => initialDamage;
    public GameObject Bullet => bullet;
    public float Cooldown => cooldown;
    public float Damage => damage;
    private void OnEnable()
    {
        initialDamage = damage;
    }
    protected void Shoot(GameObject owner, Transform target, float cone)
    {
        Vector2 pos = target.position - owner.transform.position;
        
        pos.Normalize();
        float angle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.Euler(0, 0, angle + Random.Range(-cone, cone));
        var bullet = Instantiate(this.bullet, owner.transform.position, q);
        bullet.GetComponent<Projectile>().Initialize(damage, owner.CompareTag("Enemy"));
    }
    public override void Use(GameObject owner, Transform target)
    {
        if(target == null)
        {
            return;
        }
        Shoot(owner, target, 0);
    }
    public void Upgrade(float cooldown = 0, float damage = 0)
    {
        if (cooldown > 0)
        {
            this.cooldown = cooldown;
        }
        if (damage > 0)
        {
            this.damage = damage;
        }
    }
}
