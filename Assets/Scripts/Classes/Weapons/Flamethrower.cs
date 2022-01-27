using UnityEngine;

[CreateAssetMenu(fileName = "New Flamethrower", menuName = "Weapons/Firearm/Flamethrower")]
public class Flamethrower : Firearm, IWeaponStoppable
{
    [SerializeField] private GameObject flame;
    public override void Use(Transform owner, Transform target)
    {
        Transform flame = owner.Find("Flame");
        Vector2 pos = target.position - owner.position;
        float angle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
        if (flame != null)
        {
            flame.eulerAngles = new Vector3(-angle, 90, 0);
        }
        else
        {
            flame = Instantiate(this.flame, owner).transform;
            flame.eulerAngles = new Vector3(-angle, 90, 0);
            flame.name = this.flame.name;
        }
    }
    public void Stop(Transform owner)
    {
        Transform flame = owner.Find("Flame");
        if (flame == null) return;
        flame.GetComponent<ParticleSystem>().Stop();
    }
}
public interface IWeaponStoppable
{
    void Stop(Transform owner);
}