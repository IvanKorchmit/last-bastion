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
        TechTreeUtils.techBranch = branches[0].Branch.Copy();
        positions = new List<Vector2Int>();
        GenerateTreeRecursively(TechTreeUtils.techBranch, Vector2Int.zero, 0, null, 0);
    }
    private void GenerateTreeRecursively(Branch branch, Vector2Int position, int index, GameObject previousObject, int prevLen)
    {
        if (branch.ChildBranches == null || branch.ChildBranches.Length == 0)
        {
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
            var button = CreateButton(branch.ChildBranches[i]);
            Vector2 sizeDelta = (button.transform as RectTransform).sizeDelta;
            Vector2 dir = (Vector2.down + position / sizeDelta);
            dir.x = branch.ChildBranches.Length - 1 > 0 ? (float)i / (branch.ChildBranches.Length - 1) * 2 - 1 : 0;
            dir *= sizeDelta * 1.5f;
            while (IsConflicting(Vector2Int.FloorToInt(dir)))
            {
                dir += Vector2.down * sizeDelta;
                dir.x *= 4 * sizeDelta.x;
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
            GenerateTreeRecursively(branch.ChildBranches[i], Vector2Int.FloorToInt(dir), index, button.gameObject, branch.ChildBranches.Length);
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
        button.res = branch.Researchable;
        return button;
    }
    private bool IsConflicting(Vector2Int d)
    {
        
        return positions.Contains(d);
    }
}
