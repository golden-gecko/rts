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

        cursors[OrderType.Assemble] = Assemble;
        cursors[OrderType.Attack] = Attack;
        cursors[OrderType.Construct] = Construct;
        cursors[OrderType.Destroy] = Destroy;
        cursors[OrderType.Disable] = Disable;
        cursors[OrderType.Enable] = Enable;
        cursors[OrderType.Explore] = Explore;
        cursors[OrderType.Follow] = Follow;
        cursors[OrderType.Gather] = Gather;
        cursors[OrderType.Guard] = Guard;
        cursors[OrderType.Load] = Load;
        cursors[OrderType.Move] = Move;
        cursors[OrderType.None] = None;
        cursors[OrderType.Patrol] = Patrol;
        cursors[OrderType.Produce] = Produce;
        cursors[OrderType.Rally] = Rally;
        cursors[OrderType.Research] = Research;
        cursors[OrderType.Stop] = Stop;
        cursors[OrderType.Transport] = Transport;
        cursors[OrderType.Unload] = Unload;
        cursors[OrderType.UseSkill] = UseSkill;
        cursors[OrderType.Wait] = Wait;

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
    private Texture2D Assemble { get; set; }

    [field: SerializeField]
    private Texture2D Attack { get; set; }

    [field: SerializeField]
    private Texture2D Construct { get; set; }

    [field: SerializeField]
    private Texture2D Destroy { get; set; } // TODO: Rename properties to avoid name collision.

    [field: SerializeField]
    private Texture2D Disable { get; set; }

    [field: SerializeField]
    private Texture2D Enable { get; set; }

    [field: SerializeField]
    private Texture2D Explore { get; set; }

    [field: SerializeField]
    private Texture2D Follow { get; set; }

    [field: SerializeField]
    private Texture2D Gather { get; set; }

    [field: SerializeField]
    private Texture2D Guard { get; set; }

    [field: SerializeField]
    private Texture2D Load { get; set; }

    [field: SerializeField]
    private Texture2D Move { get; set; }

    [field: SerializeField]
    private Texture2D None { get; set; }

    [field: SerializeField]
    private Texture2D Patrol { get; set; }

    [field: SerializeField]
    private Texture2D Produce { get; set; }

    [field: SerializeField]
    private Texture2D Rally { get; set; }

    [field: SerializeField]
    private Texture2D Research { get; set; }

    [field: SerializeField]
    private Texture2D Stop { get; set; }

    [field: SerializeField]
    private Texture2D Transport { get; set; }

    [field: SerializeField]
    private Texture2D Unload { get; set; }

    [field: SerializeField]
    private Texture2D UseSkill { get; set; }

    [field: SerializeField]
    private Texture2D Wait { get; set; }

    private Dictionary<OrderType, Texture2D> cursors = new Dictionary<OrderType, Texture2D>();
}
