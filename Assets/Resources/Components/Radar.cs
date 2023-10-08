using UnityEngine;

public class Radar : MyComponent
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

        if (Anti)
        {
            Map.Instance.SetVisibleByAntiRadar(parent, parent.Position, Range.Total, 1);
        }
        else
        {
            Map.Instance.SetVisibleByRadar(parent, parent.Position, Range.Total, 1);
        }
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
                if (Anti)
                {
                    Map.Instance.SetVisibleByAntiRadar(parent, parent.Position, Range.Total, 1);
                }
                else
                {
                    Map.Instance.SetVisibleByRadar(parent, parent.Position, Range.Total, 1);
                }
            }
            else
            {
                if (Anti)
                {
                    Map.Instance.SetVisibleByAntiRadar(parent, parent.Position, Range.Total, -1);
                }
                else
                {
                    Map.Instance.SetVisibleByRadar(parent, parent.Position, Range.Total, -1);
                }
            }

            PreviousEnabled = parent.Enabled;
        }

        if (PreviousPositionInt != CurrentPositionInt)
        {
            if (Anti)
            {
                Map.Instance.SetVisibleByAntiRadar(parent, PreviousPosition, Range.Total, -1);
                Map.Instance.SetVisibleByAntiRadar(parent, parent.Position, Range.Total, 1);
            }
            else
            {
                Map.Instance.SetVisibleByRadar(parent, PreviousPosition, Range.Total, -1);
                Map.Instance.SetVisibleByRadar(parent, parent.Position, Range.Total, 1);
            }

            PreviousPosition = parent.Position;
            PreviousPositionInt = CurrentPositionInt;
        }
    }

    public override string GetInfo()
    {
        return string.Format("Radar: {0}, Range: {1:0.}, Anti: {2}", base.GetInfo(), Range.Total, Anti);
    }

    public override void OnDestroy_(MyGameObject myGameObject)
    {
        if (myGameObject.Enabled)
        {
            if (Anti)
            {
                Map.Instance.SetVisibleByAntiRadar(myGameObject, Utils.ToGrid(myGameObject.Position, Config.Map.VisibilityScale), Range.Total, -1);
            }
            else
            {
                Map.Instance.SetVisibleByRadar(myGameObject, Utils.ToGrid(myGameObject.Position, Config.Map.VisibilityScale), Range.Total, -1);
            }
        }
    }

    public bool IsInRange(Vector3 position)
    {
        return Utils.IsInRange(GetComponent<MyGameObject>().Position, position, Range.Total);
    }

    [field: SerializeField]
    public Property Range { get; set; } = new Property();

    [field: SerializeField]
    public bool Anti { get; set; } = false;

    private bool PreviousEnabled = true;
    private Vector3 PreviousPosition = new Vector3();
    private Vector3Int PreviousPositionInt = new Vector3Int();
}
