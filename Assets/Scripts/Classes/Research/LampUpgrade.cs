using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using TechnologyTree.Researches.Interfaces;
[CreateAssetMenu(fileName = "New Lamp Upgrade", menuName = "Research/Technology/Lamp Upgrade")]
public class LampUpgrade : ResearchUpgrade, IFloatUpgrade
{
    private const float RADIUS_INCREASEMENT = 5.0f;
    public float GetFloatOnLevel()
    {
        return (float)level / RADIUS_INCREASEMENT;
    }
    public override void OnUpgrade()
    {
        Light2D[] lights = GameObject.FindObjectsOfType<Light2D>();
        foreach (Light2D l in lights)
        {
            if (l.lightType == Light2D.LightType.Point)
            {
                if(l.transform.root != null && l.transform.root.CompareTag("Lamp"))
                {
                    l.pointLightInnerRadius = l.pointLightInnerRadius + GetFloatOnLevel();
                }
            }
        }
    }
}



namespace TechnologyTree.Researches.Interfaces
{
    public interface IFloatUpgrade
    {
        float GetFloatOnLevel();
    }
    public interface IBoolUpgrade
    {
        bool GetBoolOnLevel();
    }
    public interface IIntUpgrade
    {
        int GetBoolOnLevel();
    }
}