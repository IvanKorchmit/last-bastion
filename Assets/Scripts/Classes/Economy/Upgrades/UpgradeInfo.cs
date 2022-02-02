[System.Serializable]
public struct UpgradeInfo
{
    public enum UpgradeType
    {
        Misc, Weapon, Unit, Unlock
    }
    public enum ParameterType
    {
        Speed, Damage, Misc
    }
    public UpgradeType type;
    public ParameterType param;
    public float value;
    public string target;
}
