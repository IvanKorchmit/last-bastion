using UnityEngine;
using TechnologyTree;
using TechnologyTree.Researches;
using UnityEngine.UI;
public class ResearchButton : MonoBehaviour, IUnlockable
{
    public Branch branch;
    private void Start()
    {
        branch.SetGO(gameObject);
    }
    public void OnClick()
    {
        if(branch.IsAvailable && !branch.IsOpen)
        {
            branch.Open();
            Unlock();
            Upgrades.Add(branch.Researchable);
            Branch[] b = branch.ChildBranches;
            for (int i = 0; i < branch.ChildBranches.Length; i++)
            {
                b[i].Available();
            }
        }
    }
    public void Unlock()
    {
        GetComponent<Image>().color = Color.green;
    }
}
public interface IUnlockable
{
    void Unlock();
}