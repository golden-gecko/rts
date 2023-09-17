using UnityEngine;

public class PowerPlant : MyComponent
{
    protected override void Start()
    {
        base.Start();

        MyGameObject parent = GetComponent<MyGameObject>();

        PreviousEnabled = parent.Enabled;
        PreviousPowered = parent.Powered;
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

        if (Power > 0.0f || parent.Powered)
        {
            Map.Instance.SetVisibleByPower(parent, parent.Position, Range, 1); // TODO: Use Power instead of 1.
        }
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

        if (PreviousPowered != parent.Powered)
        {
            if (parent.Powered)
            {
                Map.Instance.SetVisibleByPower(parent, parent.Position, Range, 1);
            }
            else
            {
                Map.Instance.SetVisibleByPower(parent, parent.Position, Range, -1);
            }

            PreviousPowered = parent.Powered;
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
        string info = string.Format("PowerPlant: {0}, Range: {1}", base.GetInfo(), Range);

        if (Power > 0.0f)
        {
            info += string.Format(" Power: {0}", Power);
        }

        return info;
    }

    [field: SerializeField]
    public float Power { get; set; } = 20.0f;

    [field: SerializeField]
    public float Range { get; set; } = 10.0f;

    [field: SerializeField]
    public float PowerUsage { get; set; } = 1.0f; // TODO: Implement.

    private bool PreviousEnabled = true;
    private bool PreviousPowered = false;
    private Vector3 PreviousPosition = new Vector3();
    private Vector3Int PreviousPositionInt = new Vector3Int();
}
