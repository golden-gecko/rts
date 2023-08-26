using UnityEngine;

public class Radar : MyComponent
{
    protected override void Update()
    {
        base.Update();

        if (Anti)
        {
            Map.Instance.SetVisibleByAntiRadar(GetComponent<MyGameObject>(), Range);
        }
        else
        {
            Map.Instance.SetVisibleByRadar(GetComponent<MyGameObject>(), Range);
        }
    }

    public override string GetInfo()
    {
        return string.Format("{0}, Range: {1:0.}, Anti: {2}", base.GetInfo(), Range, Anti);
    }

    [field: SerializeField]
    public float Range { get; set; } = 30.0f;

    [field: SerializeField]
    public bool Anti { get; set; } = false;
}
