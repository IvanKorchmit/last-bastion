using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Weapons/Firearm/Gun")]
public class Firearm : WeaponBase
{
    [SerializeField] protected float damageMult = 1;
    [SerializeField] protected float speedDiv = 1;
    [SerializeField] private float cooldown;
    [SerializeField] private GameObject bullet;
    [SerializeField] protected float damage;
    private float initialDamage;
    public float InitDamage => initialDamage;
    public GameObject Bullet => bullet;
    public float Cooldown => cooldown;
    public float Damage => damage;
    public float SpeedDivision => speedDiv;
    public float DamageMultiplier => damageMult;
    private void OnEnable()
    {
        damageMult = 1;
        speedDiv = 1;
        initialDamage = damage;
        UpgradeButton.OnUpgradeReceived += UpgradeButton_OnUpgradeReceived;
    }

    private void UpgradeButton_OnUpgradeReceived(UpgradeInfo info)
    {
        if (info.type == UpgradeInfo.UpgradeType.Weapon)
        {
            if (info.target == name)
            {
                if (info.param == UpgradeInfo.ParameterType.Damage)
                {
                    damageMult = info.value;
                }
                else if (info.param == UpgradeInfo.ParameterType.Speed)
                {
                    speedDiv = info.value;
                }
            }
        }
    }

    protected void Shoot(Transform owner, Transform target, float cone)
    {
        Vector2 pos = target.position - owner.transform.position;
        bool isEnemy = owner.root.CompareTag("Enemy");
        pos.Normalize();
        float angle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.Euler(0, 0, angle + Random.Range(-cone, cone));
        var bullet = Instantiate(this.bullet, owner.transform.position, q);
        bullet.GetComponent<Projectile>().Initialize(!isEnemy ? damage * damageMult : damage * 0.6f, isEnemy, owner.root.gameObject);
    }
    public override void Use(Transform owner, Transform target)
    {
        if(target == null)
        {
            return;
        }
        Shoot(owner, target, 0);
    }
}
