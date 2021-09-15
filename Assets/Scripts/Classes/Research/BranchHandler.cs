using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Branches", menuName = "Research/Branches")]
public class BranchHandler : ScriptableObject
{
    [SerializeField] private new string name;
    [SerializeField] private Branch branch;
    public Branch @Branch => branch;
}

[System.Serializable]
public struct Branch
{
    [SerializeField] private ResearchUpgrade researchable;
    [SerializeField] private Branch[] subBranches;
    [SerializeField] private BranchHandler commonBranch;

    public BranchHandler CommonBranch => commonBranch;
    public ResearchUpgrade Researchable => researchable;
    public Branch[] ChildBranches => (Branch[])subBranches.Clone();
}
