using UnityEngine;

public class PowerPlant : MyComponent
{
    protected override void Start()
    {
        base.Start();

        MyGameObject parent = GetComponent<MyGameObject>();

        PreviousState = parent.State;
        PreviousEnabled = parent.Enabled;
        PreviousPosition = parent.Position;
        PreviousPositionInt = Utils.ToGrid(parent.Position, Config.Map.VisibilityScale);

        if (parent.State != MyGameObjectState.Operational)
        {
            return;
        }

        if (parent.Enabled == false)
        {
            return;
        }

        if (Power > 0.0f || parent.Powered)
        {
            Map.Instance.SetVisibleByPower(parent, parent.Position, Range.Total, 1); // TODO: Use Power instead of 1.
        }

        PreviousPowered = parent.Powered;
    }

    protected override void Update()
    {
        base.Update();

        MyGameObject parent = GetComponent<MyGameObject>();

        if (PreviousState != parent.State)
        {
            PreviousState = parent.State;

            if (parent.State == MyGameObjectState.Operational)
            {
                Vector3Int CurrentPositionInt = Utils.ToGrid(parent.Position, Config.Map.VisibilityScale);

                if (parent.Enabled && parent.Powered)
                {
                    Map.Instance.SetVisibleByPower(parent, parent.Position, Range.Total, 1);
                }
                else
                {
                    Map.Instance.SetVisibleByPower(parent, parent.Position, Range.Total, -1);
                }

                PreviousEnabled = parent.Enabled;
                PreviousPowered = parent.Powered;
                PreviousPosition = parent.Position;
                PreviousPositionInt = CurrentPositionInt;
            }
            else
            {
                Map.Instance.SetVisibleByPower(parent, parent.Position, Range.Total, -1);
            }
        }
        else
        {
            Vector3Int CurrentPositionInt = Utils.ToGrid(parent.Position, Config.Map.VisibilityScale);

            if (PreviousEnabled != parent.Enabled)
            {
                if (parent.Enabled)
                {
                    Map.Instance.SetVisibleByPower(parent, parent.Position, Range.Total, 1);
                }
                else
                {
                    Map.Instance.SetVisibleByPower(parent, parent.Position, Range.Total, -1);
                }

                PreviousEnabled = parent.Enabled;
            }

            if (PreviousPowered != parent.Powered)
            {
                if (parent.Powered)
                {
                    Map.Instance.SetVisibleByPower(parent, parent.Position, Range.Total, 1);
                }
                else
                {
                    Map.Instance.SetVisibleByPower(parent, parent.Position, Range.Total, -1);
                }

                PreviousPowered = parent.Powered;
            }

            if (PreviousPositionInt != CurrentPositionInt)
            {
                Map.Instance.SetVisibleByPower(parent, PreviousPosition, Range.Total, -1);
                Map.Instance.SetVisibleByPower(parent, parent.Position, Range.Total, 1);

                PreviousPosition = parent.Position;
                PreviousPositionInt = CurrentPositionInt;
            }
        }
    }

    public override string GetInfo()
    {
        string info = string.Format("PowerPlant: {0}, Range: {1}", base.GetInfo(), Range);

        if (Power > 0.0f)
        {
            info += string.Format(" Power: {0}", Power);
        }

        return info;
    }

    public override void OnDestroy_()
    {
        base.OnDestroy_();

        MyGameObject parent = GetComponent<MyGameObject>();

        if (parent.State != MyGameObjectState.Operational)
        {
            return;
        }

        // if (parent.Enabled && (Power > 0.0f || parent.Powered))
        {
            Map.Instance.SetVisibleByPower(parent, parent.Position, Range.Total, -1);
        }
    }

    private void PowerUp()
    {
        MyGameObject parent = GetComponent<MyGameObject>();

        Map.Instance.SetVisibleByPower(parent, parent.Position, Range.Total, 1);
    }

    private void PowerDown()
    {
        MyGameObject parent = GetComponent<MyGameObject>();

        Map.Instance.SetVisibleByPower(parent, parent.Position, Range.Total, -1);
    }

    [field: SerializeField]
    public float Power { get; set; } = 20.0f;

    [field: SerializeField]
    public Property Range { get; set; } = new Property();

    [field: SerializeField]
    public float PowerUsage { get; set; } = 1.0f; // TODO: Implement.

    private MyGameObjectState PreviousState = MyGameObjectState.Operational;
    private bool PreviousEnabled = false;
    private bool PreviousPowered = false;
    private Vector3 PreviousPosition = new Vector3();
    private Vector3Int PreviousPositionInt = new Vector3Int();
}
