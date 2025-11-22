using UnityEngine;

public class Sight : MyComponent
{
    protected override void Start()
    {
        base.Start();

        MyGameObject parent = GetComponent<MyGameObject>();

        PreviousEnabled = parent.Enabled;
        PreviousPosition = parent.Position;
        PreviousPositionInt = new Vector3Int(
            Mathf.FloorToInt(parent.Position.x / Config.TerrainVisibilityScale),
            0,
            Mathf.FloorToInt(parent.Position.z / Config.TerrainVisibilityScale)
        );

        if (parent.Working == false)
        {
            return;
        }

        Map.Instance.SetVisibleBySight(parent, parent.Position, Range, 1);
    }

    protected override void Update()
    {
        base.Update();

        MyGameObject parent = GetComponent<MyGameObject>();

        Vector3Int CurrentPositionInt = new Vector3Int(
            Mathf.FloorToInt(parent.Position.x / Config.TerrainVisibilityScale),
            0,
            Mathf.FloorToInt(parent.Position.z / Config.TerrainVisibilityScale)
        );

        if (PreviousEnabled != parent.Enabled)
        {
            if (parent.Enabled)
            {
                Map.Instance.SetVisibleBySight(parent, parent.Position, Range, 1);
            }
            else
            {
                Map.Instance.SetVisibleBySight(parent, parent.Position, Range, -1);
            }


            PreviousEnabled = parent.Enabled;
        }

        if (PreviousPositionInt != CurrentPositionInt)
        {
            Map.Instance.SetVisibleBySight(parent, PreviousPosition, Range, -1);
            Map.Instance.SetVisibleBySight(parent, parent.Position, Range, 1);

            PreviousPosition = parent.Position;
            PreviousPositionInt = CurrentPositionInt;
        }
    }

    public override string GetInfo()
    {
        return string.Format("Sight: {0}, Range: {1:0.}", base.GetInfo(), Range);
    }

    public bool IsInRange(Vector3 position)
    {
        return Utils.IsInRange(GetComponent<MyGameObject>().Position, position, Range);
    }

    [field: SerializeField]
    public float Range { get; set; } = 10.0f;

    private bool PreviousEnabled = true;
    private Vector3 PreviousPosition = new Vector3();
    private Vector3Int PreviousPositionInt = new Vector3Int();
}
