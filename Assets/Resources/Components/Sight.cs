using UnityEngine;

public class Sight : MyComponent
{
    protected override void Start()
    {
        base.Start();

        MyGameObject parent = GetComponent<MyGameObject>();

        PreviousEnabled = parent.Enabled;
        PreviousPosition = parent.Position;
        PreviousPositionInt = Utils.ToGrid(parent.Position, Config.Map.VisibilityScale);

        if (parent.Enabled == false)
        {
            return;
        }

        Map.Instance.SetVisibleBySight(parent, parent.Position, Range.Total, 1);
    }

    protected override void Update()
    {
        base.Update();

        MyGameObject parent = GetComponent<MyGameObject>();

        Vector3Int CurrentPositionInt = Utils.ToGrid(parent.Position, Config.Map.VisibilityScale);

        if (PreviousEnabled != parent.Enabled)
        {
            if (parent.Enabled)
            {
                Map.Instance.SetVisibleBySight(parent, parent.Position, Range.Total, 1);
            }
            else
            {
                Map.Instance.SetVisibleBySight(parent, parent.Position, Range.Total, -1);
            }


            PreviousEnabled = parent.Enabled;
        }

        if (PreviousPositionInt != CurrentPositionInt)
        {
            Map.Instance.SetVisibleBySight(parent, PreviousPosition, Range.Total, -1);
            Map.Instance.SetVisibleBySight(parent, parent.Position, Range.Total, 1);

            PreviousPosition = parent.Position;
            PreviousPositionInt = CurrentPositionInt;
        }
    }

    public override string GetInfo()
    {
        return string.Format("Sight: {0}, Range: {1:0.}", base.GetInfo(), Range.Total);
    }

    public bool IsInRange(Vector3 position)
    {
        return Utils.IsInRange(GetComponent<MyGameObject>().Position, position, Range.Total);
    }

    [field: SerializeField]
    public Property Range { get; set; } = new Property();

    private bool PreviousEnabled = true;
    private Vector3 PreviousPosition = new Vector3();
    private Vector3Int PreviousPositionInt = new Vector3Int();
}
