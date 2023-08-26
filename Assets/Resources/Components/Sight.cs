using UnityEngine;

public class Sight : MyComponent
{
    protected override void Update()
    {
        base.Update();

        if (GetComponent<MyGameObject>().Enabled == false)
        {
            return;
        }

        Map.Instance.SetVisibleBySight(GetComponent<MyGameObject>(), Range);
    }

    public override string GetInfo()
    {
        return string.Format("{0}, Range: {1:0.}", base.GetInfo(), Range);
    }

    [field: SerializeField]
    public float Range { get; set; } = 10.0f;
}
