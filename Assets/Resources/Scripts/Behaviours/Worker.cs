using UnityEngine;

[DisallowMultipleComponent]
public class BehaviourWorker : MonoBehaviour
{
    void Awake()
    {
        MyGameObject parent = GetComponent<MyGameObject>();

        parent.OrderHandlers[OrderType.Idle] = new OrderHandlerIdleWorker();
    }
}
