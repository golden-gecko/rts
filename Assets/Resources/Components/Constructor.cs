using UnityEngine;

public class Constructor : MyComponent
{
    protected override void Start()
    {
        base.Start();

        RallyPoint = GetComponent<MyGameObject>().Entrance;
    }

    public override string GetInfo()
    {
        return string.Format("{0}, Resource Usage: {1}", base.GetInfo(), ResourceUsage);
    }

    [field: SerializeField]
    public int ResourceUsage { get; set; } = 2;

    public Vector3 RallyPoint { get; set; } = Vector3.zero;
}
