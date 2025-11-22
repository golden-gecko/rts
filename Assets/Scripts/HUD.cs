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
        UpdateMouse();
        UpdateKeyboard();
    }

    void UpdateMouse()
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
                                case OrderType.Transport:
                                    var resources = new Dictionary<string, int>
                                    {
                                        { "Coal", 10 },
                                    };

                                    var source = GameObject.Find("struct_Headquarters_A_yup").GetComponent<MyGameObject>();
                                    var target = GameObject.Find("struct_Misc_Building_B_yup").GetComponent<MyGameObject>();

                                    gameObject.Transport(source, target, resources);
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

    public void UpdateKeyboard()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
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
