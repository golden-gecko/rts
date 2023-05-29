using UnityEngine;

public class Truck : MyGameObject
{
    void Start()
    {
    }

    void Update()
    {
        if (Order == OrderType.Move)
        {
            transform.Translate((Target - transform.position).normalized * 10 * Time.deltaTime);

            if ((Target - transform.position).magnitude < 0.1f)
            {
                Order = OrderType.Idle;
            }
        }
    }
}
