using UnityEngine;

public class Radar : MyComponent
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

        if (Anti)
        {
            Map.Instance.SetVisibleByAntiRadar(parent, parent.Position, Range, 1);
        }
        else
        {
            Map.Instance.SetVisibleByRadar(parent, parent.Position, Range, 1);
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
                if (Anti)
                {
                    Map.Instance.SetVisibleByAntiRadar(parent, parent.Position, Range, 1);
                }
                else
                {
                    Map.Instance.SetVisibleByRadar(parent, parent.Position, Range, 1);
                }
            }
            else
            {
                if (Anti)
                {
                    Map.Instance.SetVisibleByAntiRadar(parent, parent.Position, Range, -1);
                }
                else
                {
                    Map.Instance.SetVisibleByRadar(parent, parent.Position, Range, -1);
                }
            }

            PreviousEnabled = parent.Enabled;
        }

        if (PreviousPositionInt != CurrentPositionInt)
        {
            if (Anti)
            {
                Map.Instance.SetVisibleByAntiRadar(parent, PreviousPosition, Range, -1);
                Map.Instance.SetVisibleByAntiRadar(parent, parent.Position, Range, 1);
            }
            else
            {
                Map.Instance.SetVisibleByRadar(parent, PreviousPosition, Range, -1);
                Map.Instance.SetVisibleByRadar(parent, parent.Position, Range, 1);
            }

            PreviousPosition = parent.Position;
            PreviousPositionInt = CurrentPositionInt;
        }
    }

    public override string GetInfo()
    {
        return string.Format("{0}, Range: {1:0.}, Anti: {2}", base.GetInfo(), Range, Anti);
    }

    public bool IsInRange(Vector3 position)
    {
        return Utils.IsInRange(GetComponent<MyGameObject>().Position, position, Range);
    }

    [field: SerializeField]
    public float Range { get; set; } = 30.0f;

    [field: SerializeField]
    public bool Anti { get; set; } = false;

    private bool PreviousEnabled = true;
    private Vector3 PreviousPosition = new Vector3();
    private Vector3Int PreviousPositionInt = new Vector3Int();
}
