using UnityEngine;

public class Radar : MyComponent
{
    protected override void Update()
    {
        base.Update();

        foreach (MyGameObject myGameObject in FindObjectsByType<MyGameObject>(FindObjectsSortMode.None))
        {
            MyGameObject parent = GetComponent<MyGameObject>();

            if (myGameObject == parent)
            {
                continue;
            }

            if (parent.IsInRadarRange(myGameObject.Position))
            {
                myGameObject.VisibleByRadar.Add(parent);
            }
            else
            {
                myGameObject.VisibleByRadar.Remove(parent);
            }

        }
    }

    public override string GetInfo()
    {
        return string.Format("{0}, Range: {1:0.}", base.GetInfo(), Range);
    }

    [field: SerializeField]
    public float Range { get; set; } = 30.0f;
}
