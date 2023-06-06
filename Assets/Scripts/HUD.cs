using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
        if (IsOverlappingUI() == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Order == OrderType.None)
                {
                    ProcessSelection();
                }
                else
                {
                    ProcessOrder();
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                ProcessOrder();
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

    void ProcessSelection()
    {
        RaycastHit hitInfo = new RaycastHit();

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
        {
            if (hitInfo.transform.tag == "Terrain")
            {
                Select(null);
            }
            else
            {
                Select(hitInfo.transform.GetComponentInParent<MyGameObject>());
            }
        }
    }

    void ProcessOrder()
    {
        RaycastHit hitInfo = new RaycastHit();

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
        {
            if (hitInfo.transform.tag == "Terrain")
            {
                IssueOrder(hitInfo.point);
            }
            else
            {
                IssueOrder(hitInfo.transform.GetComponentInParent<MyGameObject>());
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

    public bool IsOverlappingUI()
    {
        var ui = GameObject.Find("InGameMenu");
        var uiDocument = ui.GetComponent<UIDocument>();
        var rootVisualElement = uiDocument.rootVisualElement;

        foreach (var visualElement in rootVisualElement.Children())
        {
            if (visualElement.visible && visualElement.ContainsPoint(visualElement.WorldToLocal(Input.mousePosition)))
            {
                return true;
            }
        }

        return false;
    }

    public void IssueOrder(Vector3 position)
    {
        foreach (var gameObject in Selected)
        {
            if (IsMulti() == false)
            {
                gameObject.Orders.Clear();
            }

            switch (Order)
            {
                case OrderType.Attack:
                    gameObject.Attack(position);
                    break;

                case OrderType.Patrol:
                    gameObject.Patrol(position);
                    break;

                default:
                    gameObject.Move(position);
                    break;
            }
        }
    }

    public void IssueOrder(MyGameObject myGameObject)
    {
        foreach (var gameObject in Selected)
        {
            switch (Order)
            {
                case OrderType.Attack:
                    gameObject.Attack(gameObject);
                    break;

                case OrderType.Transport:
                    var resources = new Dictionary<string, int>
                    {
                        { "Coal", 10 },
                    };

                    var source = GameObject.Find("struct_Headquarters_A_yup").GetComponent<MyGameObject>();
                    var target = GameObject.Find("struct_Misc_Building_B_yup").GetComponent<MyGameObject>();

                    if (IsMulti() == false)
                    {
                        gameObject.Orders.Clear();
                    }

                    gameObject.Transport(source, target, resources);
                    break;
            }
        }
    }

    public void Select(MyGameObject myGameObject)
    {
        if (IsMulti() == false)
        {
            foreach (var i in Selected)
            {
                i.Select(false);
            }

            Selected.Clear();
        }

        if (myGameObject != null)
        {
            if (IsMulti() && Selected.Contains(myGameObject))
            {
                myGameObject.Select(false);
                Selected.Remove(myGameObject);
            }
            else
            {
                myGameObject.Select(true);
                Selected.Add(myGameObject);
            }
        }
    }
     
    bool IsMulti()
    {
        return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    }

    public List<MyGameObject> Selected { get; private set; }

    public OrderType Order
    {
        get
        {
            return order;
        }

        set
        {
            if (value == OrderType.Stop)
            {
                order = OrderType.None;

                Stop();
            }
            else
            {
                order = value;
            }
        }
    }

    private OrderType order = OrderType.None;

    public string Prefab
    {
        get
        {
            return prefab;
        }

        set
        {
            prefab = value;
        }
    }

    private string prefab = string.Empty;
}
