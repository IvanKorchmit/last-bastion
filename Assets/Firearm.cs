using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Weapons/Firearm/Gun")]
public class Firearm : Weapon
{
    [SerializeField] private float cooldown;
    [SerializeField] private GameObject bullet;
    public GameObject Bullet => bullet;
    public float Cooldown => cooldown;
    protected void Shoot(GameObject owner, Transform target, float cone)
    {
        Vector2 pos = target.position - owner.transform.position;
        pos.Normalize();
        float angle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
        var bull = Instantiate(bullet, owner.transform.position, Quaternion.Euler(0, 0, angle + Random.Range(-cone,cone)));
    }
    public override void Use(GameObject owner, Transform target)
    {
        if(target == null)
        {
            return;
        }
        Shoot(owner, target, 0);
    }
}
