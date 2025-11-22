using UnityEngine;

public class PassiveIncreasePower : Skill
{
    public override object Clone()
    {
        return new PassiveIncreasePower(Name, Range, Cooldown.Max, Value);
    }

    public PassiveIncreasePower(string name, float range, float cooldown, float value) : base(name, range, cooldown, null, true)
    {
        Value = value;
    }

    public override void Start(MyGameObject myGameObject)
    {
        base.Start(myGameObject);

        if (myGameObject.State == MyGameObjectState.Operational && myGameObject.Enabled)
        {
            PowerUp(myGameObject, myGameObject.Position);
        }

        previousState = myGameObject.State;
        previousEnabled = myGameObject.Enabled;
        previousPosition = myGameObject.Position;
    }

    public override void Update(MyGameObject myGameObject)
    {
        base.Update(myGameObject);

        if (previousState != myGameObject.State || previousEnabled != myGameObject.Enabled || Utils.ToGrid(previousPosition, Config.Map.Scale) != Utils.ToGrid(myGameObject.Position, Config.Map.Scale))
        {
            if (previousState == MyGameObjectState.Operational && previousEnabled)
            {
                PowerDown(myGameObject, previousPosition);
            }

            if (myGameObject.State == MyGameObjectState.Operational && myGameObject.Enabled)
            {
                PowerUp(myGameObject, myGameObject.Position);
            }

            previousState = myGameObject.State;
            previousEnabled = myGameObject.Enabled;
            previousPosition = myGameObject.Position;
        }
    }

    public override void OnDestroyHandler(MyGameObject myGameObject)
    {
        base.OnDestroyHandler(myGameObject);

        if (previousState == MyGameObjectState.Operational && previousEnabled)
        {
            PowerDown(myGameObject, previousPosition);
        }
    }
    private void PowerUp(MyGameObject myGameObject, Vector3 position)
    {
        Map.Instance.VisibleByPassivePower(myGameObject, position, Range, Value);
    }

    private void PowerDown(MyGameObject myGameObject, Vector3 position)
    {
        Map.Instance.VisibleByPassivePower(myGameObject, position, Range, -Value);
    }

    public float Value { get; } = 0.0f;

    private MyGameObjectState previousState;
    private bool previousEnabled;
    private Vector3 previousPosition;
}
