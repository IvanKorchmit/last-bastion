using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechnologyTree;
using TechnologyTree.Researches.Interfaces;
using UnityEngine.Experimental.Rendering.Universal;
public class UpgradeLampMono : MonoBehaviour
{
    [SerializeField] private ResearchUpgrade dependingResearch;

    private void Start()
    {
        ResearchUpgrade res = Upgrades.GetLatestUpgrade(dependingResearch);
        if (res != null && res is IFloatUpgrade f)
        {
            Light2D light = GetComponent<Light2D>();
            light.pointLightInnerRadius = light.pointLightInnerRadius + f.GetFloatOnLevel();
        }
    }
}
