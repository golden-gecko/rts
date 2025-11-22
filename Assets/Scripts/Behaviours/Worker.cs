using UnityEngine;

[DisallowMultipleComponent]
public class BehaviourWorker : Behaviour
{
    protected override void Awake()
    {
        base.Awake();

        Parent.OrderHandlers[OrderType.Idle] = new OrderHandlerIdleWorker();
    }
}
