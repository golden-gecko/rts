using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

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
            if (IsShift() == false)
            {
                selected.Stats.Add(Stats.OrdersCancelled, selected.Orders.Count);
                selected.Orders.Clear();
            }

            selected.Destroy();
        }
    }

    public void Disable()
    {
        foreach (MyGameObject selected in ActivePlayer.Selected)
        {
            if (IsShift() == false)
            {
                selected.Stats.Add(Stats.OrdersCancelled, selected.Orders.Count);
                selected.Orders.Clear();
            }

            selected.Disable();
        }
    }

    public void Enable()
    {
        foreach (MyGameObject selected in ActivePlayer.Selected)
        {
            if (IsShift() == false)
            {
                selected.Stats.Add(Stats.OrdersCancelled, selected.Orders.Count);
                selected.Orders.Clear();
            }

            selected.Enable();
        }
    }

    public void Explore()
    {
        foreach (MyGameObject selected in ActivePlayer.Selected)
        {
            if (IsShift() == false)
            {
                selected.Stats.Add(Stats.OrdersCancelled, selected.Orders.Count);
                selected.Orders.Clear();
            }

            selected.Explore();
        }
    }

    public void Gather()
    {
        foreach (MyGameObject selected in ActivePlayer.Selected)
        {
            if (IsShift() == false)
            {
                selected.Stats.Add(Stats.OrdersCancelled, selected.Orders.Count);
                selected.Orders.Clear();
            }

            selected.Gather();
        }
    }

    public void Produce(string recipe)
    {
        foreach (MyGameObject selected in ActivePlayer.Selected)
        {
            if (IsShift() == false)
            {
                selected.Stats.Add(Stats.OrdersCancelled, selected.Orders.Count);
                selected.Orders.Clear();
            }

            selected.Produce(recipe);
        }
    }

    public void Research(string technology)
    {
        foreach (MyGameObject selected in ActivePlayer.Selected)
        {
            if (IsShift() == false)
            {
                selected.Stats.Add(Stats.OrdersCancelled, selected.Orders.Count);
                selected.Orders.Clear();
            }

            selected.Research(technology);
        }
    }

    public void Stop()
    {
        foreach (MyGameObject selected in ActivePlayer.Selected)
        {
            if (IsShift() == false)
            {
                selected.Stats.Add(Stats.OrdersCancelled, selected.Orders.Count);
                selected.Orders.Clear();
            }

            selected.Stop();
        }
    }

    public void UseSkill(string skill)
    {
        foreach (MyGameObject selected in ActivePlayer.Selected)
        {
            if (IsShift() == false)
            {
                selected.Stats.Add(Stats.OrdersCancelled, selected.Orders.Count);
                selected.Orders.Clear();
            }

            selected.UseSkill(skill);
        }
    }

    public void Wait()
    {
        foreach (MyGameObject selected in ActivePlayer.Selected)
        {
            if (IsShift() == false)
            {
                selected.Stats.Add(Stats.OrdersCancelled, selected.Orders.Count);
                selected.Orders.Clear();
            }

            selected.Wait();
        }
    }

    private void Construct()
    {
        if (ActivePlayer.Selected.Count > 0)
        {
            foreach (MyGameObject selected in ActivePlayer.Selected)
            {
                if (IsShift() == false)
                {
                    selected.Stats.Add(Stats.OrdersCancelled, selected.Orders.Count);
                    selected.Orders.Clear();
                }

                selected.Construct(Prefab, Cursor.Position, Cursor.Rotation);
            }
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

    private void IssueOrder(Vector3 position)
    {
        foreach (MyGameObject selected in ActivePlayer.Selected)
        {
            if (IsShift() == false)
            {
                selected.Stats.Add(Stats.OrdersCancelled, selected.Orders.Count);
                selected.Orders.Clear();
            }

            switch (Order)
            {
                case OrderType.Attack:
                    selected.Attack(position);
                    break;

                case OrderType.Guard:
                    selected.Guard(position);
                    break;

                case OrderType.Move:
                    selected.Move(position);
                    break;

                case OrderType.Patrol:
                    selected.Patrol(position);
                    break;

                case OrderType.Rally:
                    selected.Rally(position);
                    break;
            }
        }
    }

    private void IssueOrder(MyGameObject myGameObject)
    {
        foreach (MyGameObject selected in ActivePlayer.Selected)
        {
            if (IsShift() == false)
            {
                selected.Stats.Add(Stats.OrdersCancelled, selected.Orders.Count);
                selected.Orders.Clear();
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

    private bool MouseToRaycast(out RaycastHit hitInfo, int layerMask = Physics.DefaultRaycastLayers)
    {
        return Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Config.RaycastMaxDistance, layerMask);
    }

    private bool ProcessOrder()
    {
        if (Order == OrderType.Construct)
        {
            if (Cursor.HasCorrectPosition())
            {
                Construct();

                return true;
            }
        }
        else
        {
            RaycastHit hitInfo;

            if (MouseToRaycast(out hitInfo))
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
        foreach (MyGameObject selected in ActivePlayer.Selected)
        {
            if (IsShift() == false)
            {
                selected.Stats.Add(Stats.OrdersCancelled, selected.Orders.Count);
                selected.Orders.Clear();
            }

            selected.Move(position);
        }
    }

    private void IssueOrderDefault(MyGameObject myGameObject)
    {
        foreach (MyGameObject selected in ActivePlayer.Selected)
        {
            if (IsShift() == false)
            {
                selected.Stats.Add(Stats.OrdersCancelled, selected.Orders.Count);
                selected.Orders.Clear();
            }

            if (myGameObject.Is(selected, DiplomacyState.Ally))
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

        if (MouseToRaycast(out hitInfo))
        {
            if (IsShift() == false)
            {
                SelectionClear();
            }

            if (Utils.IsTerrain(hitInfo) == false && Utils.IsWater(hitInfo) == false)
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

        if (IsShift() && ActivePlayer.Selected.Contains(myGameObject))
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

    public void SelectionClear()
    {
        foreach (MyGameObject selected in ActivePlayer.Selected)
        {
            selected.Select(false);
        }

        ActivePlayer.Selected.Clear();
    }

    private void SelectUnitInBox()
    {
        if (IsShift() == false)
        {
            SelectionClear();
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
        for (KeyCode i = KeyCode.Alpha0; i < KeyCode.Alpha9; i++)
        {
            if (IsControl() && Input.GetKeyDown(i))
            {
                ActivePlayer.AssignGroup(i);
            }
        }
    }

    private void CheckGroupActivate()
    {
        for (KeyCode i = KeyCode.Alpha0; i < KeyCode.Alpha9; i++)
        {
            if (Input.GetKeyDown(i))
            {
                ActivePlayer.SelectGroup(i);
            }
        }
    }

    public bool IsControl()
    {
        return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
    }

    public bool IsShift()
    {
        return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
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
                        if (IsShift() == false)
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
                if (IsShift() == false)
                {
                    Order = OrderType.None;
                    Prefab = string.Empty;
                }
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0.0f)
        {
            if (Order == OrderType.Construct)
            {
                Cursor.transform.Rotate(Vector3.up, 45.0f);
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0.0f)
        {
            if (Order == OrderType.Construct)
            {
                Cursor.transform.Rotate(Vector3.down, 45.0f);
            }
        }

        if (Cursor != null)
        {
            Vector3 position;

            if (Map.Instance.MouseToPosition(out position, out _))
            {
                if (Config.SnapToGrid)
                {
                    Cursor.transform.position = Utils.SnapToGrid(position);
                }
                else
                {
                    Cursor.transform.position = position;
                }
            }

            if (Cursor.HasCorrectPosition())
            {
                Cursor.GetComponentInChildren<Indicators>().OnErrorEnd();
            }
            else
            {
                Cursor.GetComponentInChildren<Indicators>().OnError();
            }
        }

        HoverOverObjects();
    }

    private void HoverOverObjects()
    {
        RaycastHit hitInfo;

        if (Cursor == null && MouseToRaycast(out hitInfo))
        {
            Hovered = hitInfo.transform.GetComponentInParent<MyGameObject>();
        }
        else
        {
            Hovered = null;
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
                Cursor = Utils.CreateGameObject(Prefab, Vector3.zero, Quaternion.identity, null, MyGameObjectState.Cursor);
                Cursor.GetComponentInChildren<Indicators>().OnConstruction();
            }
        }
    }

    public Player ActivePlayer;

    public RectTransform boxVisual;

    public MyGameObject Hovered { get; private set; }

    private bool drag = false;

    private Vector2 endPosition = Vector2.zero;

    private OrderType order = OrderType.None;

    private string prefab = string.Empty;

    private Rect selectionBox;

    private Vector2 startPosition = Vector2.zero;
}
