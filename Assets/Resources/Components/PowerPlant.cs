using UnityEngine;

public class PowerPlant : MyComponent
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

        Map.Instance.SetVisibleByPower(parent, parent.Position, Range, 1); // TODO: Use Power instead of 1.
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
                Map.Instance.SetVisibleByPower(parent, parent.Position, Range, 1);
            }
            else
            {
                Map.Instance.SetVisibleByPower(parent, parent.Position, Range, -1);
            }


            PreviousEnabled = parent.Enabled;
        }

        if (PreviousPositionInt != CurrentPositionInt)
        {
            Map.Instance.SetVisibleByPower(parent, PreviousPosition, Range, -1);
            Map.Instance.SetVisibleByPower(parent, parent.Position, Range, 1);

            PreviousPosition = parent.Position;
            PreviousPositionInt = CurrentPositionInt;
        }
    }

    public override string GetInfo()
    {
        return string.Format("{0}, Power: {1}, Range: {2}", base.GetInfo(), Power, Range);
    }

    [field: SerializeField]
    public float Power { get; set; } = 20.0f;

    [field: SerializeField]
    public float Range { get; set; } = 10.0f;

    private bool PreviousEnabled = true;
    private Vector3 PreviousPosition = new Vector3();
    private Vector3Int PreviousPositionInt = new Vector3Int();
}
