using UnityEngine;
using TechnologyTree.Researches.Interfaces;

[CreateAssetMenu(fileName = "New Weapon Damage Upgrade", menuName = "Research/Technology/Weapons/Damage")]
public class WeaponUpgradeDamage : ResearchUpgrade, IFloatUpgrade
{
    [SerializeField] private Firearm weapon;
    public float GetFloatOnLevel()
    {
        return 1 * (1 + (float)level / 10);
    }
    public override void OnUpgrade()
    {
        Firearm[] weapons = (Firearm[])Resources.FindObjectsOfTypeAll(weapon.GetType());
        foreach (Firearm w in weapons)
        {
            w.Upgrade(0, w.Damage * GetFloatOnLevel());
        }
    }
}
