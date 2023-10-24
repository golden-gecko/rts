using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Teleporter : MyComponent
{
    protected override void Start()
    {
        base.Start();

        MakeConnections();
    }

    protected override void Update()
    {
        base.Update();

        if (Parent == null)
        {
            return;
        }

        UpdateConnections();
    }

    public override void OnDestroyHandler()
    {
        base.OnDestroyHandler();

        ClearConnections();
    }

    public override string GetInfo()
    {
        return string.Format("Teleporter: {0}", base.GetInfo());
    }

    private void MakeConnections()
    {
        foreach (RaycastHit hitInfo in Utils.SphereCastAll(Parent.Position, Range.Total, Utils.GetGameObjectMask()))
        {
            Teleporter teleporter = hitInfo.transform.GetComponentInParent<Teleporter>();

            if (teleporter == null)
            {
                continue;
            }

            if (teleporter == this)
            {
                continue;
            }

            if (teleporter.Parent.Player != Parent.Player)
            {
                continue;
            }

            Vector3Int position = Utils.ToGrid(teleporter.Parent.Position, Config.Map.Scale);
            Vector3Int center = Utils.ToGrid(Parent.Position, Config.Map.Scale);

            if (Utils.IsPointInCircle(position.x, position.z, center, Mathf.FloorToInt(Range.Total / Config.Map.Scale)) == false)
            {
                continue;
            }

            Connect(teleporter);
            teleporter.Connect(this);
        }
    }

    private void UpdateConnections()
    {
        Connections.RemoveWhere(x => x == null);
    }

    private void ClearConnections()
    {
        foreach (Teleporter teleporter in Connections)
        {
            teleporter.Disconnect(this);
        }

        Connections.Clear();
    }

    private void Connect(Teleporter teleporter)
    {
        Connections.Add(teleporter);
    }

    private void Disconnect(Teleporter teleporter)
    {
        Connections.Remove(teleporter);
    }

    [field: SerializeField]
    public Property Range = new Property(100.0f);

    public HashSet<Teleporter> Connections { get; } = new HashSet<Teleporter>();
}
