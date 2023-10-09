using UnityEngine;

public class PowerPlant : MyComponent
{
    protected override void Start()
    {
        base.Start();

        previousState = parent.State;
        previousEnabled = parent.Enabled;
        previousPosition = parent.Position;
        previousPowered = parent.Powered;

        if (parent.State == MyGameObjectState.Operational && parent.Enabled && (IsRelay == false || parent.Powered))
        {
            PowerUp(parent.Position);
        }
    }

    protected override void Update()
    {
        base.Update();

        if (previousState != parent.State || previousEnabled != parent.Enabled || Utils.ToGrid(previousPosition, Config.Map.VisibilityScale) != Utils.ToGrid(parent.Position, Config.Map.VisibilityScale) || (IsRelay && previousPowered != parent.Powered))
        {
            if (previousState == MyGameObjectState.Operational && previousEnabled && (IsRelay == false || previousPowered))
            {
                PowerDown(previousPosition);
            }

            if (parent.State == MyGameObjectState.Operational && parent.Enabled && (IsRelay == false || parent.Powered))
            {
                PowerUp(parent.Position);
            }

            previousState = parent.State;
            previousEnabled = parent.Enabled;
            previousPosition = parent.Position;
            previousPowered = parent.Powered;
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

    public override void OnDestroy_()
    {
        base.OnDestroy_();

        if (previousState == MyGameObjectState.Operational && previousEnabled && (IsRelay == false || previousPowered))
        {
            PowerDown(previousPosition);
        }
    }

    private void PowerUp(Vector3 position)
    {
        if (IsRelay)
        {
            // Map.Instance.SetVisibleByPowerRelay(parent, position, Range.Total, 1);
            Map.Instance.SetVisibleByPower(parent, position, Range.Total, 1);
        }
        else
        {
            Map.Instance.SetVisibleByPower(parent, position, Range.Total, 1);
        }
    }

    private void PowerDown(Vector3 position)
    {
        if (IsRelay)
        {
            // Map.Instance.SetVisibleByPowerRelay(parent, position, Range.Total, -1);
            Map.Instance.SetVisibleByPower(parent, position, Range.Total, -1);
        }
        else
        {
            Map.Instance.SetVisibleByPower(parent, position, Range.Total, -1);
        }
    }

    [field: SerializeField]
    public float Power { get; set; } = 20.0f;

    [field: SerializeField]
    public Property Range { get; set; } = new Property();

    [field: SerializeField]
    public Property PowerUsage { get; set; } = new Property(); // TODO: Implement.

    public bool IsRelay { get => Power < 0.0f; }

    private MyGameObjectState previousState;
    private bool previousEnabled;
    private Vector3 previousPosition;
    private bool previousPowered;
}
