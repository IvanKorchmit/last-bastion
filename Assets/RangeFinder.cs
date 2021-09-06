using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
public class RangeFinder : MonoBehaviour
{
    public enum RangeType
    {
        enemy, ores
    }
    [SerializeField] private List<Transform> targets;
    [SerializeField] private RangeType type;
    private MinerAI miner;
    private CircleCollider2D circle;
    public float Radius { get => circle.radius; set => circle.radius = value; }
    public Transform ClosestTarget => targets.Count > 0 ? targets[0] : null;

    private void FixedUpdate()
    {
        targets.RemoveAll(obj => obj == null);
    }

    private void Start()
    {
        circle = GetComponent<CircleCollider2D>();
        targets = new List<Transform>();
        if (type == RangeType.ores && transform.parent.CompareTag("Player"))
        {
            miner = GetComponentInParent<MinerAI>();
        }

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (transform.parent.CompareTag("Player"))
        {
            if (type == RangeType.enemy)
            {
                if (!targets.Contains(collision.transform))
                {
                    if (collision.CompareTag("Enemy"))
                    {
                        if (CheckLighting(collision.transform))
                        {
                            targets.Add(collision.transform);
                            Resort();
                        }
                    }
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (type == RangeType.enemy)
        {
            if (transform.parent.CompareTag("Player"))
            {
                if (collision.TryGetComponent(out AIBase ai))
                {
                    if (CheckLighting(collision.transform))
                    {
                        targets.Add(collision.transform);
                        Resort();
                    }
                }
            }
            else
            {
                if (collision.CompareTag("Wall") || collision.TryGetComponent(out UnitAI ai))
                {
                    targets.Add(collision.transform);
                    Resort();
                }
            }
        }
        else if (type == RangeType.ores)
        {
            if (transform.parent.CompareTag("Player"))
            {
                if (collision.CompareTag("Ore") && miner.LastMineral == collision.GetComponent<Ore>().MineralType)
                {
                        targets.Add(collision.transform);
                        Resort();
                }
            }
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
                if (temp == item) { continue; }
                if ((temp.position-transform.position).sqrMagnitude <= (item.position - transform.position).sqrMagnitude)
                {
                    // Debug.Log($"{(temp.position - transform.position).sqrMagnitude} {temp.name} vs {(item.position - transform.position).sqrMagnitude} {item.name}");
                    temp = item;
                }
            }
            return temp;
        }
        List<Transform> newList = new List<Transform>(targets.Count);
        for (int i = 0; i < targets.Count; i++)
        {
            Transform min = Min(targets);
            targets.Remove(min);
            newList.Add(min);
        }
        targets = newList;
        
    }
    public static bool CheckLighting(Transform t)
    {
        Light2D[] lights = GameObject.FindObjectsOfType<Light2D>();
        foreach (var l in lights)
        {
            if (l.lightType == Light2D.LightType.Point)
            {
                float dist = Vector2.Distance(t.position, l.gameObject.transform.position);
                if (dist <= l.pointLightInnerRadius)
                {
                    Debug.Log($"d: {dist}\nr:{l.pointLightOuterRadius}");
                    // Debug.Log($"{t.name} at light");
                    return true;
                }
            }
        }
        return false;
    }
}
