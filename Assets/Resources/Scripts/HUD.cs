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
            if (IsShift() == false)
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
            if (IsShift() == false)
            {
                selected.Orders.Clear();
            }

            selected.Explore();
        }
    }

    public void Research(string technology)
    {
        foreach (MyGameObject selected in ActivePlayer.Selected)
        {
            if (IsShift() == false)
            {
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
                selected.Orders.Clear();
            }

            selected.Stop();
        }
    }

    public void Wait()
    {
        foreach (MyGameObject selected in ActivePlayer.Selected)
        {
            if (IsShift() == false)
            {
                selected.Orders.Clear();
            }

            selected.Wait();
        }
    }

    private void Construct(Vector3 position)
    {
        if (ActivePlayer.Selected.Count > 0)
        {
            MyGameObject myGameObject = Game.Instance.CreateGameObject(Prefab, position, ActivePlayer, MyGameObjectState.UnderConstruction);

            foreach (MyGameObject selected in ActivePlayer.Selected)
            {
                if (IsShift() == false)
                {
                    selected.Orders.Clear();
                }

                selected.Construct(prefab, myGameObject);
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
            if (IsShift() == false)
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

    private bool MouseToRaycast(out RaycastHit hitInfo)
    {
        return Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
    }

    private bool MouseToRaycastTerrain(out RaycastHit hitInfo)
    {
        return Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Config.RaycastMaxDistance, LayerMask.GetMask("Terrain"));
    }

    private void ProcessOrder()
    {
        RaycastHit hitInfo;

        if (MouseToRaycast(out hitInfo))
        {
            if (Order == OrderType.Construct)
            {
                if (hitInfo.transform.CompareTag("Terrain"))
                {
                    Construct(hitInfo.point);
                }
            }
            else
            {
                if (hitInfo.transform.CompareTag("Terrain"))
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

        if (MouseToRaycast(out hitInfo))
        {
            if (hitInfo.transform.CompareTag("Terrain"))
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
        if (IsShift() == false)
        {
            foreach (MyGameObject selected in ActivePlayer.Selected)
            {
                selected.Select(false);
            }

            ActivePlayer.Selected.Clear();
        }

        if (myGameObject != null && myGameObject.Player == ActivePlayer)
        {
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
    }

    private void SelectUnitInBox()
    {
        if (IsShift() == false)
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
        // Close application.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        
        // Show menu.
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

        // Create selection groups.
        for (KeyCode i = KeyCode.Alpha0; i < KeyCode.Alpha9; i++)
        {
            if (IsControl() && Input.GetKeyDown(i))
            {
                ActivePlayer.AssignGroup(i);
            }
        }

        // Activate selection groups.
        for (KeyCode i = KeyCode.Alpha0; i < KeyCode.Alpha9; i++)
        {
            if (Input.GetKeyDown(i))
            {
                ActivePlayer.SelectGroup(i);
            }
        }

        // Change players.
        if (IsControl() && IsShift())
        {
            Player cpu = GameObject.Find("CPU").GetComponent<Player>();
            Player gaia = GameObject.Find("Gaia").GetComponent<Player>();
            Player human = GameObject.Find("Human").GetComponent<Player>();

            if (Input.GetKeyDown(KeyCode.F1))
            {
                HUD.Instance.ActivePlayer = human;
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                HUD.Instance.ActivePlayer = cpu;
            }
            else if (Input.GetKeyDown(KeyCode.F3))
            {
                HUD.Instance.ActivePlayer = gaia;
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
                    ProcessOrder();

                    if (IsShift() == false)
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
                if (IsShift() == false)
                {
                    Order = OrderType.None;
                    Prefab = string.Empty;
                }
            }
        }

        if (Cursor != null)
        {
            RaycastHit hitInfo;

            if (MouseToRaycastTerrain(out hitInfo))
            {
                Cursor.transform.position = hitInfo.point;
            }
        }

        HoverOverObjects();
    }

    private void HoverOverObjects()
    {
        RaycastHit hitInfo;

        if (MouseToRaycast(out hitInfo))
        {
            Hovered = hitInfo.transform.GetComponentInParent<MyGameObject>();
        }
        else
        {
            Hovered = null;
        }
    }

    private MyGameObject Cursor { get; set; }

    public OrderType Order { get; set; }

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

    public PrefabConstructionType PrefabConstructionType { get; set; }

    public MyGameObject Hovered { get; private set; }

    private bool drag = false;

    private Vector2 endPosition = Vector2.zero;

    private string prefab = string.Empty;

    private Rect selectionBox;

    private Vector2 startPosition = Vector2.zero;
}
