using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RangeFinder : MonoBehaviour
{
    [SerializeField] private List<Transform> targets;
    public Transform ClosestTarget => targets.Count > 0 ? targets[0] : null;
    
    private void Start()
    {
        targets = new List<Transform>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out AIBase ai))
        {
            targets.Add(collision.transform);
            Resort();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(targets.Contains(collision.transform))
        {
            targets.Remove(collision.transform);
            Resort();
        }
    }
    private void Resort()
    { 
        Transform Min(List<Transform> l)
        {
            Transform temp = l[0];
            foreach (var item in l)
            {
                if ((temp.position-transform.position).sqrMagnitude < (item.position - transform.position).sqrMagnitude)
                {
                    temp = item;
                }
            }
            return temp;
        }
        List<Transform> newList = new List<Transform>(targets.Count);
        for (int i = 0; i < newList.Count; i++)
        {
            Transform min = Min(targets);
            targets.Remove(min);
            newList.Add(min);
        }
        
    }
}
