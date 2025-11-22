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
        Target = new Vector3(10, 0, 10);
    }

    void Update()
    {
    }

    public OrderType Order { get; set; }
    public Vector3 Target { get; set; }
}
