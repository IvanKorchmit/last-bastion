using UnityEngine;

[CreateAssetMenu(fileName = "New Shotgun", menuName = "Weapons/Firearm/Shotgun")]
public class Shotgun : Firearm
{
    [SerializeField] private int pellets;
    [SerializeField] private float cone;
    public override void Use(GameObject owner, Transform target)
    {
        for (int i = 0; i < pellets; i++)
        {
            base.Shoot(owner, target, cone);
        }
    }
}