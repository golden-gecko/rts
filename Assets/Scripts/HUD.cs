using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class HUD : MonoBehaviour
{
    void Start()
    {
        Selected = new List<MyGameObject>();

        ResetVisual();
        DrawVisual();
        DrawSelection();
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
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            startPosition = Input.mousePosition;
            drag = true;
        }

        if (Input.GetMouseButton(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (drag)
            {
                endPosition = Input.mousePosition;

                DrawVisual();
                DrawSelection();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if ((startPosition - endPosition).magnitude <= 10)
            {
                drag = false;
            }

            if (drag)
            {
                SelectUnitInBox();
            }

            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (drag == false)
            {
                if (Order == OrderType.None)
                {
                    ProcessSelection();
                }
                else
                {
                    ProcessOrder();

                    Order = OrderType.None;
                    Prefab = string.Empty;
                }
            }

            drag = false;

            ResetVisual();
            DrawVisual();
            DrawSelection();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            if (Order == OrderType.None)
            {
                ProcessOrder();
            }
            else
            {
                Order = OrderType.None;
                Prefab = string.Empty;
            }
        }

        if (Cursor != null)
        {
            var hitInfo = new RaycastHit();
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitInfo, 2000, LayerMask.GetMask("Terrain")))
            {
                Cursor.transform.position = hitInfo.point;
            }
        }
    }

    void ResetVisual()
    {
        startPosition = Vector2.zero;
        endPosition = Vector2.zero;
    }

    void DrawVisual()
    {
        var boxCenter = (startPosition + endPosition) / 2;
        boxVisual.position = boxCenter;

        var boxSize = new Vector2(Mathf.Abs(startPosition.x - endPosition.x), Mathf.Abs(startPosition.y - endPosition.y));
        boxVisual.sizeDelta = boxSize;
    }

    void DrawSelection()
    {
        if (Input.mousePosition.x < startPosition.x)
        {
            // Drag left.
            selectionBox.xMin = Input.mousePosition.x;
            selectionBox.xMax = startPosition.x;
        }
        else
        {
            // Drag right.
            selectionBox.xMin = startPosition.x;
            selectionBox.xMax = Input.mousePosition.x;
        }

        if (Input.mousePosition.y < startPosition.y)
        {
            // Drag down.
            selectionBox.yMin = Input.mousePosition.y;
            selectionBox.yMax = startPosition.y;
        }
        else
        {
            // Drag up.
            selectionBox.yMin = startPosition.y;
            selectionBox.yMax = Input.mousePosition.y;
        }
    }

    void SelectUnitInBox()
    {
        // TODO: Not very efficient. Refactor into raycast.
        foreach (var i in GameObject.FindObjectsByType<MyGameObject>(FindObjectsSortMode.None))
        {
            var screenPosition = Camera.main.WorldToScreenPoint(i.transform.position);

            if (selectionBox.Contains(screenPosition))
            {
                i.Select(true);
                Selected.Add(i);
            }
        }
    }

    public void UpdateKeyboard()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.F10))
        {
            var canvas = GameObject.Find("Canvas");

            foreach (var i in canvas.GetComponentsInChildren<UIDocument>(true))
            {
                if (i.name == "MainMenu")
                {
                    i.enabled = !i.enabled;

                    break;
                }
            }
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
            if (Order == OrderType.Construct)
            {
                if (hitInfo.transform.tag == "Terrain")
                {
                    Construct(hitInfo.point);
                }
            }
            else
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
    }

    public void Stop()
    {
        foreach (var item in Selected)
        {
            item.Stop();
        }
    }

    void Construct(Vector3 position)
    {
        var resource = Resources.Load<MyGameObject>(Prefab);
        var gameObject = Instantiate<MyGameObject>(resource, position, Quaternion.identity);

        gameObject.State = MyGameObjectState.UnderConstruction;

        foreach (var selected in Selected)
        {
            selected.Construct(gameObject, PrefabConstructionType.Structure);
        }
    }

    void IssueOrder(Vector3 position)
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

    void IssueOrder(MyGameObject myGameObject)
    {
        foreach (var gameObject in Selected)
        {
            switch (Order)
            {
                case OrderType.Attack:
                    gameObject.Attack(myGameObject);
                    break;

                case OrderType.Guard:
                    gameObject.Guard(myGameObject);
                    break;

                case OrderType.Patrol:
                    gameObject.Patrol(myGameObject);
                    break;
            }
        }
    }

    void Select(MyGameObject myGameObject)
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

    public RectTransform boxVisual;

    Rect selectionBox;

    Vector2 startPosition = Vector2.zero;

    Vector2 endPosition = Vector2.zero;

    bool drag = false;

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

            if (Cursor != null)
            {
                GameObject.Destroy(Cursor.gameObject);
            }

            if (prefab.Equals(string.Empty) == false)
            {
                var resource = Resources.Load<MyGameObject>(Prefab);
                var gameObject = Instantiate<MyGameObject>(resource, Vector3.zero, Quaternion.identity);

                gameObject.GetComponent<BoxCollider>().enabled = false;
                gameObject.GetComponent<MyGameObject>().enabled = false;

                foreach (var renderer in gameObject.GetComponentsInChildren<Renderer>())
                {
                    Color color;

                    foreach (var material in renderer.materials)
                    {
                        color = material.color;
                        color.a = 0.5f;

                        material.color = color;
                    }
                }

                Cursor = gameObject;
            }
        }
    }

    private string prefab = string.Empty;

    private MyGameObject Cursor { get; set; } = null;
}
