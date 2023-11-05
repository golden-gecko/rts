using System.Collections.Generic;
using UnityEngine;

public class Cursor2D : Singleton<Cursor2D>
{
    protected override void Awake()
    {
        base.Awake();

        Cursors[OrderType.Assemble] = CursorAssemble;
        Cursors[OrderType.AttackObject] = CursorAttack;
        Cursors[OrderType.AttackPosition] = CursorAttack;
        Cursors[OrderType.Construct] = CursorConstruct;
        Cursors[OrderType.Destroy] = CursorDestroy;
        Cursors[OrderType.Disable] = CursorDisable;
        Cursors[OrderType.Enable] = CursorEnable;
        Cursors[OrderType.Explore] = CursorExplore;
        Cursors[OrderType.Follow] = CursorFollow;
        Cursors[OrderType.GatherObject] = CursorGather;
        Cursors[OrderType.GatherResource] = CursorGather;
        Cursors[OrderType.GuardObject] = CursorGuard;
        Cursors[OrderType.GuardPosition] = CursorGuard;
        Cursors[OrderType.Load] = CursorLoad;
        Cursors[OrderType.Move] = CursorMove;
        Cursors[OrderType.None] = CursorNone;
        Cursors[OrderType.Patrol] = CursorPatrol;
        Cursors[OrderType.Produce] = CursorProduce;
        Cursors[OrderType.Rally] = CursorRally;
        Cursors[OrderType.Research] = CursorResearch;
        Cursors[OrderType.Stop] = CursorStop;
        Cursors[OrderType.Transport] = CursorTransport;
        Cursors[OrderType.Unload] = CursorUnload;
        Cursors[OrderType.UseSkill] = CursorUseSkill;
        Cursors[OrderType.Wait] = CursorWait;

        Set(OrderType.None);
    }

    public void Set(OrderType orderType)
    {
        if (Cursors.ContainsKey(orderType))
        {
            Cursor.SetCursor(Cursors[orderType], Vector2.zero, CursorMode.Auto);
        }
    }

    [SerializeField]
    private Texture2D CursorAssemble;

    [SerializeField]
    private Texture2D CursorAttack;

    [SerializeField]
    private Texture2D CursorConstruct;

    [SerializeField]
    private Texture2D CursorDestroy;

    [SerializeField]
    private Texture2D CursorDisable;

    [SerializeField]
    private Texture2D CursorEnable;

    [SerializeField]
    private Texture2D CursorExplore;

    [SerializeField]
    private Texture2D CursorFollow;

    [SerializeField]
    private Texture2D CursorGather;

    [SerializeField]
    private Texture2D CursorGuard;

    [SerializeField]
    private Texture2D CursorLoad;

    [SerializeField]
    private Texture2D CursorMove;

    [SerializeField]
    private Texture2D CursorNone;

    [SerializeField]
    private Texture2D CursorPatrol;

    [SerializeField]
    private Texture2D CursorProduce;

    [SerializeField]
    private Texture2D CursorRally;

    [SerializeField]
    private Texture2D CursorResearch;

    [SerializeField]
    private Texture2D CursorStop;

    [SerializeField]
    private Texture2D CursorTransport;

    [SerializeField]
    private Texture2D CursorUnload;

    [SerializeField]
    private Texture2D CursorUseSkill;

    [SerializeField]
    private Texture2D CursorWait;

    private Dictionary<OrderType, Texture2D> Cursors = new Dictionary<OrderType, Texture2D>();
}
