using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerPlant : MyComponent
{
    protected override void Start()
    {
        base.Start();

        MakeConnections();

        previousState = Parent.State;
        previousEnabled = Parent.Enabled;
        previousPosition = Parent.Position;
        previousProducerConnected = (IsProducer || IsProducerConnected);

        if (Parent.State == MyGameObjectState.Operational && Parent.Enabled && (IsProducer || IsProducerConnected))
        {
            PowerUp(Parent.Position);
        }
    }

    protected override void Update()
    {
        base.Update();

        if (previousState != Parent.State || previousEnabled != Parent.Enabled || Utils.ToGrid(previousPosition, Config.Map.VisibilityScale) != Utils.ToGrid(Parent.Position, Config.Map.VisibilityScale) || previousProducerConnected != (IsProducer || IsProducerConnected))
        {
            if (previousState == MyGameObjectState.Operational && previousEnabled && previousProducerConnected)
            {
                PowerDown(previousPosition);
            }

            if (Parent.State == MyGameObjectState.Operational && Parent.Enabled && (IsProducer || IsProducerConnected))
            {
                PowerUp(Parent.Position);
            }

            previousState = Parent.State;
            previousEnabled = Parent.Enabled;
            previousPosition = Parent.Position;
            previousProducerConnected = (IsProducer || IsProducerConnected);
        }
    }

    public override string GetInfo()
    {
        return string.Format("PowerPlant: {0}, Range: {1}", base.GetInfo(), Range);
    }

    public override void OnDestroy_()
    {
        base.OnDestroy_();

        Connections.Clear();

        if (previousState == MyGameObjectState.Operational && previousEnabled)
        {
            PowerDown(previousPosition);
        }
    }

    public void Connect(PowerPlant powerPlant)
    {
        Connections.Add(powerPlant);
    }

    private void MakeConnections()
    {
        Connections.Clear();

        foreach (RaycastHit hitInfo in Utils.SphereCastAll(Parent.Position, Range.Total, Utils.GetGameObjectMask()))
        {
            PowerPlant powerPlant = hitInfo.transform.GetComponentInParent<PowerPlant>();

            if (powerPlant != null && powerPlant != this && powerPlant.Parent.Player == Parent.Player)
            {
                Connect(powerPlant);
                powerPlant.Connect(this);
            }
        }
    }

    private void PowerUp(Vector3 position)
    {
        Map.Instance.SetVisibleByPower(Parent, position, Range.Total, 1);
    }

    private void PowerDown(Vector3 position)
    {
        Map.Instance.SetVisibleByPower(Parent, position, Range.Total, -1);
    }

    [field: SerializeField]
    public Property PowerGeneration { get; set; } = new Property(); // TODO: Implement.

    [field: SerializeField]
    public Property PowerUsage { get; set; } = new Property(); // TODO: Implement.

    [field: SerializeField]
    public Property Range { get; set; } = new Property();

    public bool IsRelay { get => PowerGeneration.Total <= 0.0f && PowerUsage.Total <= 0.0f; }

    public bool IsProducer { get => PowerGeneration.Total > 0.0f; }

    public bool IsConsumer { get => PowerUsage.Total > 0.0f; }

    public bool IsProducerConnected
    {
        get
        {
            HashSet<PowerPlant> queue = new HashSet<PowerPlant>(Connections);
            HashSet<PowerPlant> visited = new HashSet<PowerPlant>();

            while (queue.Count > 0)
            {
                PowerPlant powerPlant = queue.First();
                queue.Remove(powerPlant);

                if (visited.Contains(powerPlant))
                {
                    continue;
                }

                visited.Add(powerPlant);

                if (powerPlant.Parent.State != MyGameObjectState.Operational || powerPlant.Parent.Enabled == false)
                {
                    continue;
                }

                if (powerPlant.IsProducer)
                {
                    return true;
                }

                foreach (PowerPlant i in powerPlant.Connections)
                {
                    queue.Add(i);
                }
            }

            return false;
        }
    }

    private HashSet<PowerPlant> Connections { get; } = new HashSet<PowerPlant>();

    private MyGameObjectState previousState;
    private bool previousEnabled;
    private Vector3 previousPosition;
    private bool previousProducerConnected;
}
