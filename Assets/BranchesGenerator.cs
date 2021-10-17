using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TechnologyTree;
public class BranchesGenerator : MonoBehaviour
{
    [SerializeField] private BranchHandler[] branches;
    [SerializeField] private GameObject skillButtonPrefab;
    [SerializeField] private GameObject line;
    private List<Vector2Int> positions;
    private void Start()
    {
        //StartCoroutine(GenerateTreeRecursively(branches[0].Branch, Vector2.one, 0, null, branches[0].Branch.ChildBranches.Length));
        for (int i = 0; i < branches.Length; i++)
        {
            var copy = Instantiate(branches[i]);
            branches[i] = copy;
        }
        positions = new List<Vector2Int>();
        for (int i = 0; i < branches.Length; i++)
        {
            float ang = ((float)i / branches.Length) * 360f;
            float fract = branches.Length - 1 > 0 ? (float)i / (branches.Length - 1) * 2 - 1 : 0;
            Vector2 dir = (Vector2)(Quaternion.Euler(0, 0, ang) * Vector2.right).normalized;
            Debug.Log(dir + " " + ang);
            GenerateTreeRecursively(branches[i].Branch, new Vector2Int((int)fract * 10, 0), 0, null, dir);

        }
    }
    private void GenerateTreeRecursively(Branch branch, Vector2Int position, int index, GameObject previousObject, Vector2 branchGrowthDirection)
    {
        if ((branch.ChildBranches == null || branch.ChildBranches.Length == 0) && branch.CommonBranch == null)
        {
            return;
        }
        else if (branch.CommonBranch != null)
        {
            Debug.Log("Found common");  
            branch = branch.CommonBranch.Branch.Copy();
            GenerateTreeRecursively(branch, Vector2Int.FloorToInt(position), index, previousObject, branchGrowthDirection);
            return;
        }
        if (index == 0)
        {
            branch.Available();
        }
        index++;
        for (int i = 0; i < branch.ChildBranches.Length; i++)
        {
            #region main logic
            float fract = (branch.ChildBranches.Length - 1 > 0 ? (float)i / (branch.ChildBranches.Length - 1) * 2 - 1 : 0);
            var button = CreateButton(branch.ChildBranches[i]);
            Vector2 sizeDelta = (button.transform as RectTransform).sizeDelta;
            Vector2 fractVec = new Vector2(fract, fract);
            Vector2 dir = (branchGrowthDirection + position / sizeDelta);
            dir *= sizeDelta * 1.5f;
            while (IsConflicting(Vector2Int.FloorToInt(dir)))
            {
                dir += branchGrowthDirection * sizeDelta;
            }
            if (index == 1)
            {
                button.branch.Open();
                for (int c = 0; c < button.branch.ChildBranches.Length; c++)
                {
                    button.branch.ChildBranches[c].Available();
                }
                button.Unlock();
            }
            (button.transform as RectTransform).anchoredPosition = dir;

            if (previousObject != null)
            {
                GenerateLine((button.transform as RectTransform).anchoredPosition, (previousObject.transform as RectTransform).anchoredPosition);
            }
            positions.Add(Vector2Int.FloorToInt(dir));
            #endregion
            GenerateTreeRecursively(branch.ChildBranches[i], Vector2Int.FloorToInt(dir), index, button.gameObject, branchGrowthDirection);
        }
    }
    private void GenerateLine(Vector2 a, Vector2 b)
    {
        var l = Instantiate(line, transform).GetComponent<Image>();
        var rT = l.transform as RectTransform;
        Vector2 dir = (b - a).normalized;
        float dist = Vector2.Distance(a, b);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rT.anchorMin = new Vector2();
        rT.anchorMax = new Vector2();
        rT.anchoredPosition = a;
        rT.sizeDelta = new Vector2(dist, rT.sizeDelta.y);
        rT.localEulerAngles = new Vector3(0, 0, angle);
        l.transform.SetAsLastSibling();
    }
    private ResearchButton CreateButton(Branch branch)
    {
        var button = Instantiate(skillButtonPrefab, transform).GetComponent<ResearchButton>();
        button.branch = branch;
        return button;
    }
    private bool IsConflicting(Vector2Int d)
    {
        return positions.Contains(d);
    }
}
