using UnityEngine;

public class PowerPlant : MyComponent
{
    protected override void Start()
    {
        base.Start();

        MyGameObject parent = GetComponent<MyGameObject>();

        PreviousEnabled = parent.Enabled;
        PreviousPosition = parent.Position;
        PreviousPositionInt = Utils.ToGrid(parent.Position, Config.Map.VisibilityScale);

        if (parent.Enabled == false)
        {
            return;
        }

        if (Power > 0.0f || parent.Powered)
        {
            Map.Instance.SetVisibleByPower(parent, parent.Position, Range.Total, 1); // TODO: Use Power instead of 1.
        }

        PreviousPowered = parent.Powered;
    }

    protected override void Update()
    {
        base.Update();

        MyGameObject parent = GetComponent<MyGameObject>();

        Vector3Int CurrentPositionInt = Utils.ToGrid(parent.Position, Config.Map.VisibilityScale);

        if (PreviousEnabled != parent.Enabled)
        {
            if (parent.Enabled)
            {
                Map.Instance.SetVisibleByPower(parent, parent.Position, Range.Total, 1);
            }
            else
            {
                Map.Instance.SetVisibleByPower(parent, parent.Position, Range.Total, -1);
            }


            PreviousEnabled = parent.Enabled;
        }

        if (PreviousPowered != parent.Powered)
        {
            if (parent.Powered)
            {
                Map.Instance.SetVisibleByPower(parent, parent.Position, Range.Total, 1);
            }
            else
            {
                Map.Instance.SetVisibleByPower(parent, parent.Position, Range.Total, -1);
            }

            PreviousPowered = parent.Powered;
        }

        if (PreviousPositionInt != CurrentPositionInt)
        {
            Map.Instance.SetVisibleByPower(parent, PreviousPosition, Range.Total, -1);
            Map.Instance.SetVisibleByPower(parent, parent.Position, Range.Total, 1);

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

    public override void OnDestroy_(MyGameObject myGameObject)
    {
        // if (myGameObject.Enabled && (Power > 0.0f || myGameObject.Powered))
        {
            Map.Instance.SetVisibleByPower(myGameObject, Utils.ToGrid(myGameObject.Position, Config.Map.VisibilityScale), Range.Total, -1);
        }
    }

    [field: SerializeField]
    public float Power { get; set; } = 20.0f;

    [field: SerializeField]
    public Property Range { get; set; } = new Property();

    [field: SerializeField]
    public float PowerUsage { get; set; } = 1.0f; // TODO: Implement.

    private bool PreviousEnabled = true;
    private bool PreviousPowered = false;
    private Vector3 PreviousPosition = new Vector3();
    private Vector3Int PreviousPositionInt = new Vector3Int();
}
