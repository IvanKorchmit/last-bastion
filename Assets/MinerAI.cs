using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerAI : UnitAI
{
    [SerializeField]
    private Ore oreTarget;
    [SerializeField]
    private ShopUtils.MineralType lastMineral;
    public ShopUtils.MineralType LastMineral
    {
        get
        {
            return lastMineral;
        }
    }
    [SerializeField]
    private int quantity;
    private bool isMining = true;
    public Ore OreTarget
    {
        set
        {
            oreTarget = value;
            if (oreTarget == null && range.ClosestTarget != null)
            {
                lastMineral = oreTarget.MineralType;
                Debug.Log(true);
                return;
            }
            lastMineral = ShopUtils.MineralType.Armenederdrnazite;
        }
        get
        {
            if (oreTarget == null && range.ClosestTarget != null)
            {
                oreTarget = range.ClosestTarget.GetComponent<Ore>();
                lastMineral = oreTarget.MineralType;
                return oreTarget;
            }
            lastMineral = ShopUtils.MineralType.Armenederdrnazite;
            return oreTarget;
        }
    }
    public void AddQuantity(int q)
    {
        if (quantity + q > 100)
        {
            quantity = 100;
        }
        else
        {
            quantity += q;
        }
    }
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        transform.Translate(base.MoveDirectionP);
        MoveIgnoreRange();
        Mine();
        QuantityCheck();
    }
    private void QuantityCheck()
    {
        if (quantity >= 100 && isMining)
        {
            Debug.Log("Going back");
            FindPath(transform.position, GameObject.Find(WavesUtils.COLONY_PATH).transform.position);
            isMining = false;
        }
        else if (!isMining && (path == null || path.path.Count == 0))
        {
            ShopUtils.GainResource(quantity, lastMineral);
            quantity = 0;
            isMining = true;
            if (OreTarget != null)
            {
                FindPath(transform.position, OreTarget.transform.position);
            }
            else
            {
                FindPath(transform.position, GameObject.FindGameObjectWithTag("Ore").transform.position);
            }
        }
    }
    protected void MoveIgnoreRange()
    {
        if (isMining && OreTarget != null && range.ClosestTarget != null && OreTarget.gameObject == range.ClosestTarget.gameObject)
        {
            moveDirection = Vector2.zero;
            return;
        }
        if (path != null)
        {
            moveDirection = ((Vector3)path.path[0].position - transform.position).normalized;
            if (Vector2.Distance(transform.position, (Vector3)path.path[0].position) <= 0.5f)
            {
                if (path.path.Count - 1 > 0)
                {
                    path.path.RemoveAt(0);
                }
                else
                {
                    moveDirection = Vector2.zero;
                    path = null;
                }
            }
        }
    }
    private void Mine()
    {
        if (OreTarget != null && range.ClosestTarget == oreTarget.transform && isMining)
        {
            if (oreTarget.SetMiner(this))
            {
                TimerUtils.AddTimer(1f, () =>
                {
                    oreTarget.Damage(5);
                });
            }
        }
        else if (OreTarget == null && isMining && (path.path == null || path.path.Count == 0))
        {
            FindPath(transform.position, GameObject.FindGameObjectWithTag("Ore").transform.position);
        }
    }
}
