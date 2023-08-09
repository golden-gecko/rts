using UnityEngine;

public class Repair : Skill
{
    public Repair(string name, float range, float value) : base(name)
    {
        Range = range;
        Value = value;
    }

    public override void Execute(MyGameObject myGameObject)
    {
        foreach (MyGameObject target in GameObject.FindObjectsByType<MyGameObject>(FindObjectsSortMode.None))
        {
            if (target.IsEnemy(myGameObject) == false)
            {
                continue;
            }

            if (target.IsInRange(myGameObject.Position, Range) == false)
            {
                continue;
            }

            target.OnRepair(Value);
        }

        Object.Instantiate(Resources.Load("CFXR Water Ripples"), myGameObject.Position, Quaternion.identity);
    }

    public float Range { get; } = 0.0f;
 
    public float Value { get; } = 0.0f;
}
