using UnityEngine;

public class BehaviourWorker : MonoBehaviour // TODO: Create behaviour handlers.
{
    void Awake()
    {
        MyGameObject parent = GetComponent<MyGameObject>();

        parent.OrderHandlers[OrderType.Idle] = new OrderHandlerIdleWorker();
    }
}
