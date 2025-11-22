using UnityEngine;

public class Worker : MonoBehaviour // TODO: Create behaviour handlers.
{
    void Awake()
    {
        MyGameObject parent = GetComponent<MyGameObject>();

        parent.OrderHandlers[OrderType.Idle] = new OrderHandlerIdleWorker();
    }
}
