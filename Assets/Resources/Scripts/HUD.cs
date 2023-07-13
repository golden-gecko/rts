using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

        foreach (MyGameObject selected in ActivePlayer.Selected)
        {
            if (selected == null)
            {
                destroyed.Add(selected);
            }
        }

        foreach (MyGameObject myGameObject in destroyed)
        {
            ActivePlayer.Selected.Remove(myGameObject);
        }

        UpdateMouse();
        UpdateKeyboard();
    }

    public void Assemble(string prefab)
    {
        foreach (MyGameObject selected in ActivePlayer.Selected)
        {
            selected.Assemble(prefab);
        }
    }

    public void Destroy()
    {
        foreach (MyGameObject selected in ActivePlayer.Selected)
        {
            if (IsMulti() == false)
            {
                selected.Orders.Clear();
            }

            selected.Destroy();
        }
    }

    public void Explore()
    {
        foreach (MyGameObject selected in ActivePlayer.Selected)
        {
            if (IsMulti() == false)
            {
                selected.Orders.Clear();
            }

            selected.Explore();
        }
    }

    public void Research()
    {
        foreach (MyGameObject selected in ActivePlayer.Selected)
        {
            if (IsMulti() == false)
            {
                selected.Orders.Clear();
            }

            selected.Research(Technology);
        }
    }

    public void Stop()
    {
        foreach (MyGameObject selected in ActivePlayer.Selected)
        {
            if (IsMulti() == false)
            {
                selected.Orders.Clear();
            }

            selected.Stop();
        }
    }

    public void Wait()
    {
        foreach (MyGameObject selected in ActivePlayer.Selected)
        {
            if (IsMulti() == false)
            {
                selected.Orders.Clear();
            }

            selected.Wait();
        }
    }

    private void Construct(Vector3 position)
    {
        foreach (MyGameObject selected in ActivePlayer.Selected)
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
        foreach (MyGameObject selected in ActivePlayer.Selected)
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
        foreach (MyGameObject selected in ActivePlayer.Selected)
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

    private void Select(MyGameObject myGameObject)
    {
        if (IsMulti() == false)
        {
            foreach (MyGameObject selected in ActivePlayer.Selected)
            {
                selected.Select(false);
            }

            ActivePlayer.Selected.Clear();
        }

        if (gameObject != null && myGameObject.Player == ActivePlayer)
        {
            if (IsMulti() && ActivePlayer.Selected.Contains(myGameObject))
            {
                myGameObject.Select(false);
                ActivePlayer.Selected.Remove(myGameObject);
            }
            else
            {
                myGameObject.Select(true);
                ActivePlayer.Selected.Add(myGameObject);
            }
        }
    }

    private void SelectUnitInBox()
    {
        if (IsMulti() == false)
        {
            foreach (MyGameObject selected in ActivePlayer.Selected)
            {
                selected.Select(false);
            }

            ActivePlayer.Selected.Clear();
        }

        foreach (MyGameObject myGameObject in GameObject.FindObjectsByType<MyGameObject>(FindObjectsSortMode.None)) // TODO: Not very efficient. Refactor into raycast.
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(myGameObject.Position);

            if (selectionBox.Contains(screenPosition))
            {
                if (myGameObject.Player == ActivePlayer)
                {
                    myGameObject.Select(true);

                    ActivePlayer.Selected.Add(myGameObject);
                }
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
            if (MainMenu.Instance.gameObject.activeInHierarchy || SceneMenu.Instance.gameObject.activeInHierarchy)
            {
                MainMenu.Instance.gameObject.SetActive(false);
                SceneMenu.Instance.gameObject.SetActive(false);
            }
            else
            {
                MainMenu.Instance.gameObject.SetActive(true);
            }
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

        HoverOverObjects();
    }

    private void HoverOverObjects()
    {
        Hovered = null;
        RaycastHit hitInfo;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
        {
            MyGameObject myGameObject = hitInfo.transform.GetComponentInParent<MyGameObject>();

            if (myGameObject != null)
            {
                Hovered = myGameObject;
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

                case OrderType.Explore:
                    order = OrderType.None;
                    Explore();
                    break;

                case OrderType.Research:
                    order = OrderType.None;
                    Research();
                    break;

                case OrderType.Stop:
                    order = OrderType.None;
                    Stop();
                    break;

                case OrderType.Wait:
                    order = OrderType.None;
                    Wait();
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

            if (prefab.Equals(string.Empty) == false)
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

    public Player ActivePlayer;

    public RectTransform boxVisual;

    public string Technology { get; set; }

    public PrefabConstructionType PrefabConstructionType { get; set; }

    public MyGameObject Hovered { get; private set; }

    private bool drag = false;

    private Vector2 endPosition = Vector2.zero;

    private OrderType order = OrderType.None;

    private string prefab = string.Empty;

    private Rect selectionBox;

    private Vector2 startPosition = Vector2.zero;
}
