using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class HUD : MonoBehaviour
{
    public static HUD Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        ResetVisual();
        DrawVisual();
        DrawSelection();
    }

    private void Update()
    {
        HashSet<MyGameObject> destroyed = new HashSet<MyGameObject>();

        foreach (MyGameObject selected in Selected)
        {
            if (selected == null)
            {
                destroyed.Add(selected);
            }
        }

        foreach (MyGameObject x in destroyed)
        {
            Selected.Remove(x);
        }

        UpdateMouse();
        UpdateKeyboard();
    }

    public void Assemble(string prefab)
    {
        foreach (MyGameObject selected in Selected)
        {
            selected.Assemble(prefab);
        }
    }

    public void Destroy()
    {
        foreach (MyGameObject selected in Selected)
        {
            selected.Destroy();
        }
    }

    public void Stop()
    {
        foreach (MyGameObject selected in Selected)
        {
            selected.Stop();
        }
    }

    private void Construct(Vector3 position)
    {
        foreach (MyGameObject selected in Selected)
        {
            if (IsMulti() == false)
            {
                selected.Orders.Clear();
            }

            selected.Construct(prefab, position);
        }
    }

    private void DrawSelection()
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

    private void DrawVisual()
    {
        Vector2 boxCenter = (startPosition + endPosition) / 2;
        boxVisual.position = boxCenter;

        Vector2 boxSize = new Vector2(Mathf.Abs(startPosition.x - endPosition.x), Mathf.Abs(startPosition.y - endPosition.y));
        boxVisual.sizeDelta = boxSize;
    }

    private bool IsMulti()
    {
        return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    }

    private void IssueOrder(Vector3 position)
    {
        foreach (MyGameObject selected in Selected)
        {
            if (IsMulti() == false)
            {
                selected.Orders.Clear();
            }

            switch (Order)
            {
                case OrderType.Attack:
                    selected.Attack(position);
                    break;

                case OrderType.Patrol:
                    selected.Patrol(position);
                    break;

                case OrderType.Rally:
                    selected.Rally(position, 0);
                    break;

                default:
                    selected.Move(position);
                    break;
            }
        }
    }

    private void IssueOrder(MyGameObject myGameObject)
    {
        foreach (MyGameObject selected in Selected)
        {
            if (IsMulti() == false)
            {
                selected.Orders.Clear();
            }

            switch (Order)
            {
                case OrderType.Attack:
                    selected.Attack(myGameObject);
                    break;

                case OrderType.Guard:
                    selected.Guard(myGameObject);
                    break;

                case OrderType.Follow:
                    selected.Follow(myGameObject);
                    break;

                default:
                    if (selected.IsAlly(myGameObject))
                    {
                        selected.Follow(myGameObject);
                    }
                    else if (selected.IsEnemy(myGameObject))
                    {
                        selected.Attack(myGameObject);
                    }
                    break;
            }
        }
    }

    private void ProcessOrder()
    {
        RaycastHit hitInfo;

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

    private void ProcessSelection()
    {
        RaycastHit hitInfo;

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

    private void ResetVisual()
    {
        startPosition = Vector2.zero;
        endPosition = Vector2.zero;
    }

    private void Select(MyGameObject gameObject)
    {
        if (IsMulti() == false)
        {
            foreach (MyGameObject selected in Selected)
            {
                selected.Select(false);
            }

            Selected.Clear();
        }

        if (gameObject != null)
        {
            if (IsMulti() && Selected.Contains(gameObject))
            {
                gameObject.Select(false);
                Selected.Remove(gameObject);
            }
            else
            {
                gameObject.Select(true);
                Selected.Add(gameObject);
            }
        }
    }

    private void SelectUnitInBox()
    {
        if (IsMulti() == false)
        {
            foreach (MyGameObject selected in Selected)
            {
                selected.Select(false);
            }

            Selected.Clear();
        }

        foreach (MyGameObject i in GameObject.FindObjectsByType<MyGameObject>(FindObjectsSortMode.None)) // TODO: Not very efficient. Refactor into raycast.
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(i.transform.position);

            if (selectionBox.Contains(screenPosition))
            {
                i.Select(true);
                Selected.Add(i);
            }
        }
    }

    private void UpdateKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        else if (Input.GetKeyDown(KeyCode.F10))
        {
            MainMenu.Instance.gameObject.SetActive(!MainMenu.Instance.gameObject.activeInHierarchy);
        }
    }

    private void UpdateMouse()
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

                    if (IsMulti() == false)
                    {
                        Order = OrderType.None;
                        Prefab = string.Empty;
                    }
                }
            }

            drag = false;

            ResetVisual();
            DrawVisual();
            DrawSelection();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (Order == OrderType.None)
            {
                ProcessOrder();
            }
            else
            {
                if (IsMulti() == false)
                {
                    Order = OrderType.None;
                    Prefab = string.Empty;
                }
            }
        }

        if (Cursor != null)
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitInfo, 2000, LayerMask.GetMask("Terrain")))
            {
                Cursor.transform.position = hitInfo.point;
            }
        }
    }

    private MyGameObject Cursor { get; set; }

    public OrderType Order
    {
        get
        {
            return order;
        }

        set
        {
            switch (value)
            {
                case OrderType.Destroy:
                    order = OrderType.None;
                    Destroy();
                    break;

                case OrderType.Stop:
                    order = OrderType.None;
                    Stop();
                    break;

                default:
                    order = value;
                    break;
            }
        }
    }

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

            if (prefab.Equals(string.Empty) == false && PrefabConstructionType == PrefabConstructionType.Structure)
            {
                MyGameObject resource = Resources.Load<MyGameObject>(Prefab);
                MyGameObject myGameObject = Instantiate<MyGameObject>(resource, Vector3.zero, Quaternion.identity);

                myGameObject.GetComponent<BoxCollider>().enabled = false;
                myGameObject.GetComponent<MyGameObject>().enabled = false;

                foreach (Renderer renderer in myGameObject.GetComponentsInChildren<Renderer>())
                {
                    Color color;

                    foreach (Material material in renderer.materials)
                    {
                        color = material.color;
                        color.a = 0.5f;

                        material.color = color;
                    }
                }

                Cursor = myGameObject;
            }
        }
    }

    public PrefabConstructionType PrefabConstructionType { get; set; }

    public List<MyGameObject> Selected { get; } = new List<MyGameObject>();

    public RectTransform boxVisual;

    private bool drag = false;

    private Vector2 endPosition = Vector2.zero;

    private OrderType order = OrderType.None;

    private string prefab = string.Empty;

    private Rect selectionBox;

    private Vector2 startPosition = Vector2.zero;
}
