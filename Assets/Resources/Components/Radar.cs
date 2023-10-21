using UnityEngine;

public class Radar : MyComponent
{
    protected override void Start()
    {
        base.Start();

        if (Parent.State == MyGameObjectState.Operational && Parent.Enabled)
        {
            PowerUp(Parent.Position);
        }

        previousState = Parent.State;
        previousEnabled = Parent.Enabled;
        previousPosition = Parent.Position;
    }

    protected override void Update()
    {
        base.Update();

        if (previousState != Parent.State || previousEnabled != Parent.Enabled || Utils.ToGrid(previousPosition, Config.Map.Scale) != Utils.ToGrid(Parent.Position, Config.Map.Scale))
        {
            if (previousState == MyGameObjectState.Operational && previousEnabled)
            {
                PowerDown(previousPosition);
            }

            if (Parent.State == MyGameObjectState.Operational && Parent.Enabled)
            {
                PowerUp(Parent.Position);
            }

            previousState = Parent.State;
            previousEnabled = Parent.Enabled;
            previousPosition = Parent.Position;
        }
    }

    public override string GetInfo()
    {
        return string.Format("Radar: {0}, Range: {1:0.}, Anti: {2}", base.GetInfo(), Range.Total, Anti);
    }

    public override void OnDestroy_()
    {
        base.OnDestroy_();

        if (previousState == MyGameObjectState.Operational && previousEnabled)
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
            Map.Instance.SetVisibleByAntiRadar(Parent, position, Range.Total, 1);
        }
        else
        {
            Map.Instance.SetVisibleByRadar(Parent, position, Range.Total, 1);
        }
    }

    private void PowerDown(Vector3 position)
    {
        if (Anti)
        {
            Map.Instance.SetVisibleByAntiRadar(Parent, position, Range.Total, -1);
        }
        else
        {
            Map.Instance.SetVisibleByRadar(Parent, position, Range.Total, -1);
        }
    }

    [field: SerializeField]
    public Property Range { get; private set; } = new Property();

    [field: SerializeField]
    public bool Anti { get; private set; } = false;

    private MyGameObjectState previousState;
    private bool previousEnabled;
    private Vector3 previousPosition;
}
