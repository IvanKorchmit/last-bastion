using UnityEngine;
using TechnologyTree;
using UnityEngine.UI;
public class ResearchButton : MonoBehaviour, IUnlockable
{
    public ResearchUpgrade res;
    public Branch branch;
    private void Start()
    {
        branch.SetGO(gameObject);
    }
    public void OnClick()
    {
        if(branch.IsAvailable)
        {
            branch.Open();
            branch.ButtonReference.GetComponent<IUnlockable>().Unlock();
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