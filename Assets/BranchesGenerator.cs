using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BranchesGenerator : MonoBehaviour
{
    [SerializeField] private BranchHandler[] branches;
    [SerializeField] private GameObject skillButtonPrefab;
    [SerializeField] private GameObject line;
    [SerializeField] private GameObject layoutObject;
    private void Start()
    {
        //StartCoroutine(GenerateTreeRecursively(branches[0].Branch, Vector2.one, 0, null, branches[0].Branch.ChildBranches.Length));
        GenerateTreeRecursively(branches[0].Branch, Vector2.one, 0, null, branches[0].Branch.ChildBranches.Length);
    }
    private void GenerateTreeRecursively(Branch branch, Vector2 position, int index, GameObject previousObject, int prevBranchLength)
    {
        if (branch.ChildBranches == null || branch.ChildBranches.Length == 0)
        {
            return;
        }
        index++;
        // yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < branch.ChildBranches.Length; i++)
        {
            var button = Instantiate(skillButtonPrefab,transform);
            (button.transform as RectTransform).anchoredPosition = position;
            Vector2 sizeDelta = (button.transform as RectTransform).sizeDelta;
            Vector2 dir = (Vector2.down + (position / sizeDelta));
            dir.x += branch.ChildBranches.Length - 1 > 0 ? ((float)i / (branch.ChildBranches.Length - 1) * 2 - 1) * 2 : 0;
            dir *= sizeDelta * 1.5f;   

            Debug.Log($"{dir} {i} {index}");


                

            button.GetComponentInChildren<TextMeshProUGUI>().text = $"{index} {i}";
            if(previousObject != null)
            {
                GenerateLine((button.transform as RectTransform).anchoredPosition, (previousObject.transform as RectTransform).anchoredPosition);
            }
            GenerateTreeRecursively(branch.ChildBranches[i], dir, index, button, branch.ChildBranches.Length);
            //StartCoroutine(GenerateTreeRecursively(branch.ChildBranches[i], dir, index, button, branch.ChildBranches.Length));
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

}
