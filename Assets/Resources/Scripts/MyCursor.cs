using System.Collections.Generic;
using UnityEngine;

public class MyCursor : MonoBehaviour
{
    public static MyCursor Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        cursors[OrderType.Assemble] = CursorAssemble;
        cursors[OrderType.Attack] = CursorAttack;
        cursors[OrderType.Construct] = CursorConstruct;
        cursors[OrderType.Destroy] = CursorDestroy;
        cursors[OrderType.Disable] = CursorDisable;
        cursors[OrderType.Enable] = CursorEnable;
        cursors[OrderType.Explore] = CursorExplore;
        cursors[OrderType.Follow] = CursorFollow;
        cursors[OrderType.Gather] = CursorGather;
        cursors[OrderType.Guard] = CursorGuard;
        cursors[OrderType.Load] = CursorLoad;
        cursors[OrderType.Move] = CursorMove;
        cursors[OrderType.None] = CursorNone;
        cursors[OrderType.Patrol] = CursorPatrol;
        cursors[OrderType.Produce] = CursorProduce;
        cursors[OrderType.Rally] = CursorRally;
        cursors[OrderType.Research] = CursorResearch;
        cursors[OrderType.Stop] = CursorStop;
        cursors[OrderType.Transport] = CursorTransport;
        cursors[OrderType.Unload] = CursorUnload;
        cursors[OrderType.UseSkill] = CursorUseSkill;
        cursors[OrderType.Wait] = CursorWait;

        Set(OrderType.None);
    }

    public void Set(OrderType orderType)
    {
        if (cursors.ContainsKey(orderType))
        {
            Cursor.SetCursor(cursors[orderType], Vector2.zero, CursorMode.Auto);
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

    private Dictionary<OrderType, Texture2D> cursors = new Dictionary<OrderType, Texture2D>();
}
