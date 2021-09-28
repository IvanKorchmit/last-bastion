using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Branches", menuName = "Research/Branches")]
public class BranchHandler : ScriptableObject
{
    [SerializeField] private new string name;
    [SerializeField] private Branch branch;
    public Branch @Branch => branch;
    public string Name => name;
}

[System.Serializable]
public class Branch
{
    [SerializeField] private ResearchUpgrade researchable;
    [SerializeField] private Branch[] subBranches;
    [SerializeField] private BranchHandler commonBranch;
    [SerializeField] private bool isAvailable;
    [SerializeField] private GameObject buttonReference;
    private bool open;

    public GameObject ButtonReference => buttonReference;
    public void SetGO(GameObject gameObj)
    {
        buttonReference = gameObj;
    }

    public bool IsAvailable => isAvailable;
    public bool IsOpen => open;
    public void Available()
    {
        isAvailable = true;

    }
    public void Open()
    {
        open = true;
        foreach (var b in subBranches)
        {
            b.Available();
        }
    }

    public Branch Copy()
    {
        Branch[] c = new Branch[subBranches.Length];
        for (int i = 0; i < c.Length; i++)
        {
            c[i] = subBranches[i].Copy();
        }
        return new Branch(researchable, c, buttonReference, isAvailable, open);
    }
    public Branch(ResearchUpgrade researchable, Branch[] subBranches, GameObject buttonReference, bool isAvailable, bool open)
    {
        this.researchable = researchable;
        this.subBranches = subBranches;
        this.buttonReference = buttonReference;
        this.isAvailable = isAvailable;
        this.open = open;
    }
    public BranchHandler CommonBranch => commonBranch;
    public ResearchUpgrade Researchable => researchable;
    public Branch[] ChildBranches => subBranches;
}
