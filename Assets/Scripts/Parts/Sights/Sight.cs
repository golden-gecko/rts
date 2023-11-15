using UnityEngine;

[DisallowMultipleComponent]
public class Sight : Part
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

        if (Alive == false)
        {
            return;
        }

        if (previousState != Parent.State || previousEnabled != Parent.Enabled || Utils.ToGrid(previousPosition, Config.Map.Scale) != Utils.ToGrid(Parent.Position, Config.Map.Scale)) // TODO: Add alive state.
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

    public override void OnDestroyHandler()
    {
        base.OnDestroyHandler();

        if (previousState == MyGameObjectState.Operational && previousEnabled)
        {
            PowerDown(previousPosition);
        }
    }

    public override string GetInfo()
    {
        return string.Format("Sight - {0}, Range: {1:0.}", base.GetInfo(), Range.Total);
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
    public Property Range { get; private set; } = new Property(10.0f);

    private MyGameObjectState previousState;
    private bool previousEnabled;
    private Vector3 previousPosition;
}
