using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    void Start()
    {
        Selected = new List<MyGameObject>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = new RaycastHit();

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
            {
                if (hitInfo.transform.tag == "Terrain")
                {
                    foreach (var gameObject in Selected)
                    {
                        switch (Order)
                        {
                            case OrderType.Patrol:
                                gameObject.Patrol(hitInfo.point);
                                break;

                            default:
                                gameObject.Move(hitInfo.point);
                                break;
                        }
                    }
                }
                else
                {
                    var myGameObject = hitInfo.transform.GetComponentInParent<MyGameObject>();

                    if (myGameObject != null && Order != OrderType.None)
                    {
                        foreach (var gameObject in Selected)
                        {
                            switch (Order)
                            {
                                case OrderType.Load:
                                    var resources = new Dictionary<string, int>
                                    {
                                        { "Coal", 10 },
                                    };

                                    gameObject.Load(myGameObject, resources);
                                    break;
                            }
                        }
                    }
                    else
                    {
                        Selected.Clear();

                        if (myGameObject != null)
                        {
                            Selected.Add(myGameObject);
                        }
                    }
                }
            }
        }
    }
    public void Stop()
    {
        foreach (var item in Selected)
        {
            item.Stop();
        }
    }

    public List<MyGameObject> Selected { get; private set; }

    public OrderType Order { get; set; } = OrderType.None;
}
