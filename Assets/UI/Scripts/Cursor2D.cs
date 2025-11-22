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

    [field: SerializeField]
    private Texture2D CursorAssemble { get; set; }

    [field: SerializeField]
    private Texture2D CursorAttack { get; set; }

    [field: SerializeField]
    private Texture2D CursorConstruct { get; set; }

    [field: SerializeField]
    private Texture2D CursorDestroy { get; set; }

    [field: SerializeField]
    private Texture2D CursorDisable { get; set; }

    [field: SerializeField]
    private Texture2D CursorEnable { get; set; }

    [field: SerializeField]
    private Texture2D CursorExplore { get; set; }

    [field: SerializeField]
    private Texture2D CursorFollow { get; set; }

    [field: SerializeField]
    private Texture2D CursorGather { get; set; }

    [field: SerializeField]
    private Texture2D CursorGuard { get; set; }

    [field: SerializeField]
    private Texture2D CursorLoad { get; set; }

    [field: SerializeField]
    private Texture2D CursorMove { get; set; }

    [field: SerializeField]
    private Texture2D CursorNone { get; set; }

    [field: SerializeField]
    private Texture2D CursorPatrol { get; set; }

    [field: SerializeField]
    private Texture2D CursorProduce { get; set; }

    [field: SerializeField]
    private Texture2D CursorRally { get; set; }

    [field: SerializeField]
    private Texture2D CursorResearch { get; set; }

    [field: SerializeField]
    private Texture2D CursorStop { get; set; }

    [field: SerializeField]
    private Texture2D CursorTransport { get; set; }

    [field: SerializeField]
    private Texture2D CursorUnload { get; set; }

    [field: SerializeField]
    private Texture2D CursorUseSkill { get; set; }

    [field: SerializeField]
    private Texture2D CursorWait { get; set; }

    private Dictionary<OrderType, Texture2D> Cursors { get; } = new Dictionary<OrderType, Texture2D>();
}
