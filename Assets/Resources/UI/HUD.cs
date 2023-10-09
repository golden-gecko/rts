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
        DragReset();
        DragDraw();

        DiplomacyMenu.Instance.Show(false);
        GameMenu.Instance.Show(true);
        MainMenu.Instance.Show(false);
        SceneMenu.Instance.Show(false);
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
            UI_Menu.Instance.OnMenu();
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

            DragStart();
        }

        if (Input.GetMouseButton(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (drag)
            {
                DragUpdate();
                DragDraw();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            DragEnd();

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

            DragReset();
            DragDraw();
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
            else if (MyInput.GetShift() == false)
            {
                Order = OrderType.None;
                Prefab = string.Empty;
            }
        }

        UpdateGameObjectUnderMouse();
    }

    private void UpdateGameObjectUnderMouse()
    {
        if (Cursor3D.Instance.Visible)
        {
            Hovered = null;
        }
        else
        {
            Hovered = Utils.RaycastGameObjectFromMouse();
        }
    }

    private void DragStart()
    {
        startPosition = Input.mousePosition;
        drag = true;
    }

    private void DragUpdate()
    {
        endPosition = Input.mousePosition;
    }

    private void DragEnd()
    {
        if ((startPosition - endPosition).magnitude <= 10)
        {
            drag = false;
        }

        if (drag)
        {
            SelectUnitInBox();
        }
    }

    private void DragReset()
    {
        startPosition = Vector2.zero;
        endPosition = Vector2.zero;
    }

    private void DragDraw()
    {
        Vector2 boxCenter = (startPosition + endPosition) / 2;
        BoxVisual.position = boxCenter;

        Vector2 boxSize = new Vector2(Mathf.Abs(startPosition.x - endPosition.x), Mathf.Abs(startPosition.y - endPosition.y));
        BoxVisual.sizeDelta = boxSize;

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

    private MyGameObject GetGameObjectUnderMouse()
    {
        RaycastHit hitInfo;

        if (Utils.RaycastFromMouse(out hitInfo, Utils.GetGameObjectMask()) == false)
        {
            return null;
        }

        MyGameObject myGameObject = hitInfo.transform.GetComponentInParent<MyGameObject>();

        if (myGameObject == null)
        {
            return null;
        }

        if (myGameObject.VisibilityState == MyGameObjectVisibilityState.Hidden)
        {
            return null;
        }

        return myGameObject;
    }

    private void SelectUnitInBox()
    {
        if (MyInput.GetShift() == false)
        {
            ActivePlayer.Selection.Clear();
        }

        foreach (MyGameObject myGameObject in FindObjectsByType<MyGameObject>(FindObjectsSortMode.None)) // TODO: Not very efficient. Refactor into raycast.
        {
            if (selectionBox.Contains(Camera.main.WorldToScreenPoint(myGameObject.Position)))
            {
                Select(myGameObject);
            }
        }
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
            if (Cursor3D.Instance.HasCorrectPosition())
            {
                MyGameObject myGameObject = Utils.CreateGameObject(Prefab, Cursor3D.Instance.Position, Cursor3D.Instance.Rotation, ActivePlayer, MyGameObjectState.UnderConstruction);

                ActivePlayer.Selection.Construct(myGameObject, MyInput.GetShift());

                return true;
            }
        }
        else
        {
            MyGameObject myGameObject = Utils.RaycastGameObjectFromMouse();

            if (myGameObject != null)
            {
                if (Order == OrderType.None)
                {
                    IssueOrderDefault(myGameObject);
                }
                else
                {
                    IssueOrder(myGameObject);
                }

                return true;
            }
            else if (Utils.RaycastFromMouse(out RaycastHit hitInfo, Utils.GetMapMask()))
            {
                if (Order == OrderType.None)
                {
                    IssueOrderDefault(hitInfo.point);
                }
                else
                {
                    IssueOrder(hitInfo.point);
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

    public OrderType Order
    {
        get
        {
            return order;
        }

        set
        {
            order = value;

            Cursor2D.Instance.Set(order);
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

            if (Cursor3D.Instance.Visible)
            {
                Cursor3D.Instance.Destroy();
            }

            if (prefab.Length > 0)
            {
                Cursor3D.Instance.Create(prefab, ActivePlayer);
            }
        }
    }

    [field: SerializeField]
    public Player ActivePlayer { get; private set; }

    [field: SerializeField]
    public RectTransform BoxVisual { get; private set; }

    public MyGameObject Hovered { get; private set; }

    private OrderType order = OrderType.None;

    private string prefab = string.Empty;

    private Rect selectionBox;

    private bool drag = false;

    private Vector2 startPosition = Vector2.zero;

    private Vector2 endPosition = Vector2.zero;
}
