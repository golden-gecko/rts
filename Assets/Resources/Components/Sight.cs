using UnityEngine;

public class Sight : MyComponent
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

            if (parent.IsInVisibilityRange(myGameObject.Position))
            {
                myGameObject.VisibleBySight[parent.Player].Add(parent);
            }
            else
            {
                myGameObject.VisibleBySight[parent.Player].Remove(parent);
            }

        }
    }

    public override string GetInfo()
    {
        return string.Format("{0}, Range: {1:0.}", base.GetInfo(), Range);
    }

    [field: SerializeField]
    public float Range { get; set; } = 10.0f;
}
