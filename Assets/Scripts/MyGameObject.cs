using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class MyGameObject : MonoBehaviour
{
    protected virtual void Start()
    {
        var whitelist = new HashSet<OrderType> { OrderType.Idle, OrderType.Stop };

        Orders = new OrderContainer(whitelist);
        Resources = new Dictionary<string, Resource>();
    }

    protected virtual void Update()
    {
        if (Orders.Count > 0)
        {
            var order = Orders.First();

            if (Orders.Contains(order.Type))
            {
                switch (order.Type)
                {
                    case OrderType.Produce:
                        OnProduceOrder();
                        break;

                    case OrderType.Idle:
                        OnIdleOrder();
                        break;

                    case OrderType.Move:
                        OnMoveOrder();
                        break;

                    case OrderType.Stop:
                        OnStopOrder();
                        break;
                }
            }
            else
            {
                Orders.RemoveAt(0);
            }
        }
        else
        {
            Idle();
        }
    }

    #region Orders
    public void Idle()
    {
        // TODO: Restore.
        // Orders.Add(new Order(OrderType.Idle));
    }

    public void Move(MyGameObject target)
    {
        Orders.Add(new Order(OrderType.Move, target));
    }

    public void Move(Vector3 target)
    {
        Orders.Add(new Order(OrderType.Move, target));
    }

    public void Produce()
    {
    }

    public void Stop()
    {
        Orders.Insert(0, new Order(OrderType.Stop));
    }
    #endregion

    #region Handlers

    void OnIdleOrder()
    {
        var objects = GameObject.FindGameObjectsWithTag("Resource");

        foreach (var item in objects)
        {
            Orders.Add(new Order(OrderType.Move, item.transform.position));

            break;
        }

        Orders.RemoveAt(0);
    }

    void OnMoveOrder()
    {
        var order = Orders.First();

        var distanceToTarget = (order.TargetPosition - transform.position).magnitude;
        var distanceToTravel = 10 * Time.deltaTime;

        if (distanceToTarget > distanceToTravel)
        {
            var direction = order.TargetPosition - transform.position;
            direction.y = 0;
            direction.Normalize();

            transform.LookAt(order.TargetPosition);
            transform.Translate(Vector3.forward * distanceToTravel);
        }
        else
        {
            var direction = order.TargetPosition - transform.position;
            direction.y = 0;
            direction.Normalize();

            transform.LookAt(order.TargetPosition);
            transform.position = order.TargetPosition;

            Orders.RemoveAt(0);
        }
    }

    void OnProduceOrder()
    {
        var order = Orders.First();

        if (order.TargetGameObject == null)
        {
            Orders.RemoveAt(0);
        }
        else
        {
            order.Timer += Time.deltaTime;

            if (order.Timer >= order.TimerMax)
            {
                Resources["Wood"].Value += 1;

                Orders.RemoveAt(0);
            }
        }
    }

    void OnStopOrder()
    {
        Orders.Clear();
    }
    #endregion

    public OrderContainer Orders { get; private set; }

    public Dictionary<string, Resource> Resources { get; private set; }
}
