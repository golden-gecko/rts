using UnityEngine;

public enum OrderType
{
    Idle = 0,
    Move = 1,
}

public class MyGameObject : MonoBehaviour
{
    void Start()
    {
    }

    void Update()
    {
    }

    public OrderType Order { get => _orderType; set => _orderType = value; }

    public Vector3 Target { get => _target; set => _target = value; }

    [SerializeField]
    private OrderType _orderType = OrderType.Idle;

    [SerializeField]
    private Vector3 _target = new Vector3(0, 0, 0);
}
