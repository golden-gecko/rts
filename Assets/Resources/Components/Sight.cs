using UnityEngine;

public class Sight : MyComponent
{
    protected override void Start()
    {
        base.Start();

        previousState = Parent.State;
        previousEnabled = Parent.Enabled;
        previousPosition = Parent.Position;

        if (Parent.State == MyGameObjectState.Operational && Parent.Enabled)
        {
            PowerUp(Parent.Position);
        }
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
        return string.Format("Sight: {0}, Range: {1:0.}", base.GetInfo(), Range.Total);
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
        Map.Instance.SetVisibleBySight(Parent, position, Range.Total, 1);
    }

    private void PowerDown(Vector3 position)
    {
        Map.Instance.SetVisibleBySight(Parent, position, Range.Total, -1);
    }

    [field: SerializeField]
    public Property Range { get; private set; } = new Property();

    private MyGameObjectState previousState;
    private bool previousEnabled;
    private Vector3 previousPosition;
}
