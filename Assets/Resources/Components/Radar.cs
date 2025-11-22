using UnityEngine;

public class Radar : MyComponent
{
    protected override void Start()
    {
        base.Start();

        previousState = parent.State;
        previousEnabled = parent.Enabled;
        previousPosition = parent.Position;

        if (parent.State == MyGameObjectState.Operational && parent.Enabled)
        {
            PowerUp(parent.Position);
        }
    }

    protected override void Update()
    {
        base.Update();

        if (previousState != parent.State || previousEnabled != parent.Enabled || Utils.ToGrid(previousPosition, Config.Map.VisibilityScale) != Utils.ToGrid(parent.Position, Config.Map.VisibilityScale))
        {
            if (previousState == MyGameObjectState.Operational && previousEnabled)
            {
                PowerDown(previousPosition);
            }

            if (parent.State == MyGameObjectState.Operational && parent.Enabled)
            {
                PowerUp(parent.Position);
            }

            previousState = parent.State;
            previousEnabled = parent.Enabled;
            previousPosition = parent.Position;
        }
    }

    public override string GetInfo()
    {
        return string.Format("Radar: {0}, Range: {1:0.}, Anti: {2}", base.GetInfo(), Range.Total, Anti);
    }

    public override void OnDestroy_()
    {
        base.OnDestroy_();

        if (previousState == MyGameObjectState.Operational || previousEnabled)
        {
            PowerDown(previousPosition);
        }
    }

    public bool IsInRange(Vector3 position)
    {
        return Utils.IsInRange(GetComponent<MyGameObject>().Position, position, Range.Total);
    }

    private void PowerUp(Vector3 position)
    {
        if (Anti)
        {
            Map.Instance.SetVisibleByAntiRadar(parent, position, Range.Total, 1);
        }
        else
        {
            Map.Instance.SetVisibleByRadar(parent, position, Range.Total, 1);
        }
    }

    private void PowerDown(Vector3 position)
    {
        if (Anti)
        {
            Map.Instance.SetVisibleByAntiRadar(parent, position, Range.Total, -1);
        }
        else
        {
            Map.Instance.SetVisibleByRadar(parent, position, Range.Total, -1);
        }
    }

    [field: SerializeField]
    public Property Range { get; set; } = new Property();

    [field: SerializeField]
    public bool Anti { get; set; } = false;

    private MyGameObjectState previousState;
    private bool previousEnabled;
    private Vector3 previousPosition;
}
