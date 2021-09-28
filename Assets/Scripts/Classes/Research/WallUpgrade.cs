using UnityEngine;
using TechnologyTree.Researches.Interfaces;

[CreateAssetMenu(fileName = "New Wall Upgrade", menuName = "Research/Technology/Wall Upgrade")]
public class WallUpgrade : ResearchUpgrade, IFloatUpgrade
{
    private const float WALL_HEALTH_INC = 10.5f;
    public float GetFloatOnLevel()
    {
        return WALL_HEALTH_INC * level;
    }

    public override void OnUpgrade()
    {
        foreach (var w in GameObject.FindGameObjectsWithTag("Wall"))
        {
            Wall wall = w.GetComponent<Wall>();
            wall.SetDurability(wall.MaxHealth + GetFloatOnLevel());
        }
    }
}
