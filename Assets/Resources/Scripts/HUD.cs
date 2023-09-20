using UnityEngine;
using UnityEngine.EventSystems;

public class HUD : MonoBehaviour
{
    public static HUD Instance { get; private set; }

    void Awake()
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

    void Start()
    {
        ResetVisual();
        DrawVisual();
        DrawSelection();
    }

    void Update()
    {
        UpdateKeyboard();
        UpdateMouse();
    }

    private void UpdateKeyboard()
    {
        CheckCloseApplication();
        CheckShowMenu();
        CheckGroupCreate();
        CheckGroupActivate();
    }

    private void CheckCloseApplication()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void CheckShowMenu()
    {
        if (Input.GetKeyDown(KeyCode.F10))
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

    private void CheckGroupCreate()
    {
        for (KeyCode i = KeyCode.Alpha0; i <= KeyCode.Alpha9; i++)
        {
            if (MyInput.GetControl() && Input.GetKeyDown(i))
            {
                ActivePlayer.AssignGroup(i);
            }
        }
    }

    private void CheckGroupActivate()
    {
        for (KeyCode i = KeyCode.Alpha0; i <= KeyCode.Alpha9; i++)
        {
            if (MyInput.GetControl() == false && Input.GetKeyDown(i))
            {
                ActivePlayer.SelectGroup(i, MyInput.GetShift());
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
                    if (ProcessOrder())
                    {
                        if (MyInput.GetShift() == false)
                        {
                            Order = OrderType.None;
                            Prefab = string.Empty;
                        }
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
                if (MyInput.GetShift() == false)
                {
                    Order = OrderType.None;
                    Prefab = string.Empty;
                }
            }
        }

        UpdateCursor();
        UpdateGameObjectUnderMouse();
    }

    private void UpdateCursor()
    {
        if (Cursor == null)
        {
            return;
        }

        // Rotate.
        if (Input.GetAxis("Mouse ScrollWheel") > 0.0f)
        {
            Cursor.transform.Rotate(Vector3.up, Config.CursorRotateStep);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0.0f)
        {
            Cursor.transform.Rotate(Vector3.down, Config.CursorRotateStep);
        }

        // Follow mouse.
        Vector3 position;

        if (Map.Instance.MouseToPosition(Cursor, out position, out _))
        {
            Cursor.transform.position = Config.SnapToGrid ? Utils.SnapToGrid(position) : position;
        }

        if (Cursor.HasCorrectPosition())
        {
            Cursor.Indicators.OnErrorEnd();
        }
        else
        {
            Cursor.Indicators.OnError();
        }
    }

    private void UpdateGameObjectUnderMouse()
    {
        if (Cursor == null)
        {
            Hovered = GetGameObjectUnderMouse();
        }
        else
        {
            Hovered = null;
        }
    }

    private MyGameObject GetGameObjectUnderMouse()
    {
        RaycastHit hitInfo;

        if (Utils.RaycastFromMouse(out hitInfo, LayerMask.GetMask("GameObject")))
        {
            return hitInfo.transform.GetComponentInParent<MyGameObject>();
        }

        return null;
    }

    private void SelectUnitInBox()
    {
        if (MyInput.GetShift() == false)
        {
            ActivePlayer.Selection.Clear();
        }

        foreach (MyGameObject myGameObject in FindObjectsByType<MyGameObject>(FindObjectsSortMode.None)) // TODO: Not very efficient. Refactor into raycast.
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(myGameObject.Position);

            if (selectionBox.Contains(screenPosition) == false)
            {
                continue;
            }

            Select(myGameObject);
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
        BoxVisual.position = boxCenter;

        Vector2 boxSize = new Vector2(Mathf.Abs(startPosition.x - endPosition.x), Mathf.Abs(startPosition.y - endPosition.y));
        BoxVisual.sizeDelta = boxSize;
    }

    private void IssueOrder(Vector3 position)
    {
        switch (Order)
        {
            case OrderType.Attack:
                ActivePlayer.Selection.Attack(position, MyInput.GetShift());
                break;

            case OrderType.Guard:
                ActivePlayer.Selection.Guard(position, MyInput.GetShift());
                break;

            case OrderType.Move:
                ActivePlayer.Selection.Move(position, MyInput.GetShift());
                break;

            case OrderType.Patrol:
                ActivePlayer.Selection.Patrol(position, MyInput.GetShift());
                break;

            case OrderType.Rally:
                ActivePlayer.Selection.Rally(position, MyInput.GetShift());
                break;
        }
    }

    private void IssueOrder(MyGameObject myGameObject)
    {
        foreach (MyGameObject selected in ActivePlayer.Selection.Items)
        {
            if (MyInput.GetShift() == false)
            {
                selected.ClearOrders();
            }

            switch (Order)
            {
                // case OrderType.Assemble: // TODO: Implement.

                case OrderType.Attack:
                    selected.Attack(myGameObject);
                    break;

                case OrderType.Construct:
                    selected.Construct(myGameObject);
                    break;

                case OrderType.Gather:
                    selected.Gather(myGameObject);
                    break;

                case OrderType.Guard:
                    selected.Guard(myGameObject);
                    break;

                case OrderType.Follow:
                    selected.Follow(myGameObject);
                    break;
            }
        }
    }

    private bool ProcessOrder()
    {
        if (Order == OrderType.Construct)
        {
            if (Cursor.HasCorrectPosition())
            {
                MyGameObject myGameObject = Utils.CreateGameObject(Prefab, Cursor.Position, Cursor.Rotation, ActivePlayer, MyGameObjectState.UnderConstruction);

                ActivePlayer.Selection.Construct(myGameObject, MyInput.GetShift());

                return true;
            }
        }
        else
        {
            RaycastHit hitInfo;

            if (Utils.RaycastFromMouse(out hitInfo))
            {
                if (Utils.IsTerrain(hitInfo) || Utils.IsWater(hitInfo))
                {
                    if (Order == OrderType.None)
                    {
                        IssueOrderDefault(hitInfo.point);
                    }
                    else
                    {
                        IssueOrder(hitInfo.point);
                    }
                }
                else
                {
                    if (Order == OrderType.None)
                    {
                        IssueOrderDefault(hitInfo.transform.GetComponentInParent<MyGameObject>());
                    }
                    else
                    {
                        IssueOrder(hitInfo.transform.GetComponentInParent<MyGameObject>());
                    }
                }

                return true;
            }
        }

        return false;
    }

    private void IssueOrderDefault(Vector3 position)
    {
        foreach (MyGameObject selected in ActivePlayer.Selection.Items)
        {
            if (MyInput.GetShift() == false)
            {
                selected.ClearOrders();
            }

            selected.Move(position);
        }
    }

    private void IssueOrderDefault(MyGameObject myGameObject)
    {
        foreach (MyGameObject selected in ActivePlayer.Selection.Items)
        {
            if (MyInput.GetShift() == false)
            {
                selected.ClearOrders();
            }

            if (myGameObject.Is(selected, DiplomacyState.Ally) && myGameObject.State == MyGameObjectState.UnderAssembly)
            {
                // TODO: Implement.
            }
            else if (myGameObject.Is(selected, DiplomacyState.Ally) && myGameObject.State == MyGameObjectState.UnderConstruction)
            {
                selected.Construct(myGameObject);
            }
            else if (myGameObject.Is(selected, DiplomacyState.Ally))
            {
                selected.Follow(myGameObject);
            }
            else if (myGameObject.Is(selected, DiplomacyState.Enemy))
            {
                selected.Attack(myGameObject);
            }
            else if (myGameObject.Gatherable)
            {
                selected.Gather(myGameObject);
            }
        }
    }

    private void ProcessSelection()
    {
        RaycastHit hitInfo;

        if (Utils.RaycastFromMouse(out hitInfo) == false)
        {
            return;
        }

        if (MyInput.GetShift() == false)
        {
            ActivePlayer.Selection.Clear();
        }

        if (Utils.IsTerrain(hitInfo) == false && Utils.IsWater(hitInfo) == false)
        {
            Select(hitInfo.transform.GetComponentInParent<MyGameObject>());
        }
    }

    private void ResetVisual()
    {
        startPosition = Vector2.zero;
        endPosition = Vector2.zero;
    }

    public void Select(MyGameObject myGameObject)
    {
        if (myGameObject == null)
        {
            return;
        }

        if (myGameObject.Player != ActivePlayer)
        {
            return;
        }

        if (myGameObject.Selectable == false)
        {
            return;
        }

        if (MyInput.GetShift() && ActivePlayer.Selection.Contains(myGameObject))
        {
            myGameObject.Select(false);
            ActivePlayer.Selection.Remove(myGameObject);
        }
        else
        {
            myGameObject.Select(true);
            ActivePlayer.Selection.Add(myGameObject);
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
            order = value;

            MyCursor.Instance.Set(order);
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
                Destroy(Cursor.gameObject);
            }

            if (prefab.Equals(string.Empty) == false)
            {
                Cursor = Utils.CreateGameObject(Prefab, Vector3.zero, Quaternion.identity, ActivePlayer, MyGameObjectState.Cursor);
                Cursor.gameObject.layer = LayerMask.NameToLayer("Cursor");
            }
        }
    }

    [field: SerializeField]
    public Player ActivePlayer { get; private set; }

    [field: SerializeField]
    public RectTransform BoxVisual { get; private set; }

    public MyGameObject Hovered { get; private set; }

    private bool drag = false;

    private Vector2 endPosition = Vector2.zero;

    private OrderType order = OrderType.None;

    private string prefab = string.Empty;

    private Rect selectionBox;

    private Vector2 startPosition = Vector2.zero;
}
